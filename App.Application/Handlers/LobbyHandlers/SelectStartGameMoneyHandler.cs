using App.Application.Repositories;
using App.Application.Repositories.RoomRepository;
using App.Application.Repositories.UnitOfWork;
using App.Contracts.Requests;
using App.Contracts.Responses;
using App.SignalR.Commands.LobbyCommands;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.LobbyHandlers;

public class SelectStartGameMoneyHandler : ICommandHandler<SelectStartGameMoneyCommand, bool>
{
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly ILogger<SelectStartGameMoneyHandler> _logger;
    private readonly IRoomRepository _roomRepository;
    private readonly IPlayerInfoRepository _playerInfoRepository;
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    private sealed record Result(bool Success, SelectStartGameMoneyResponse? Selector, string? ErrorMessage);

    public SelectStartGameMoneyHandler(
        IHubContext<GlobalHub, IGlobalHub> hubContext,
        ILogger<SelectStartGameMoneyHandler> logger,
        IRoomRepository roomRepository,
        IAppUnitOfWork unitOfWork,
        IMediator mediator,
        IPlayerInfoRepository playerInfoRepository)
    {
        _hubContext = hubContext;
        _logger = logger;
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _playerInfoRepository = playerInfoRepository;
    }

    public async ValueTask<bool> Handle(SelectStartGameMoneyCommand command, CancellationToken cT)
    {
        command.Request.Deconstruct(out RoomRequest roomRequest, out Guid playerId, out Guid? roomId);

        var playerInfo = await _playerInfoRepository.GetPlayerInfoByIdAsNoTrackingAsync(playerId, cT);

        var selectorResult = ComputeSelectStartGameMoney(
            startBid: roomRequest.StartBid,
            minBid: roomRequest.MinBid,
            maxBid: roomRequest.MaxBid,
            playerMoney: playerInfo.Money,
            roomId: roomId);

        if (selectorResult.Success)
        {
            await _hubContext.Clients.User(playerId.ToString()).ReceiveOwn_SelectStartGameMoney(selectorResult.Selector!, cT);
            
            return true;
        }

        var notificationResponse = new NotificationResponse(NotificationId: Guid.NewGuid(), NotificationText: selectorResult.ErrorMessage!);
        await _hubContext.Clients.User(playerId.ToString()).ReceiveClient_Notification(notificationResponse, cT);

        return false;
    }

    // 100 10/20 
    // 350 recommend
    // 240 lower bound
    // 540 upper bound

    private Result ComputeSelectStartGameMoney(int startBid, int minBid, int maxBid, int playerMoney, Guid? roomId)
    {
        var recommendedValue = startBid * 2 + minBid * 5 + maxBid * 5;
        var lowerBound = startBid * 1.5 + minBid * 3 + maxBid * 3;
        var upperBound = startBid * 3 + minBid * 8 + maxBid * 8;
        int availableUpperBound = upperBound;

        bool success = playerMoney >= lowerBound;

        if (success && playerMoney < recommendedValue)
        {
            recommendedValue = playerMoney;
            availableUpperBound = playerMoney;
        }
        else if (success && playerMoney > recommendedValue && playerMoney < upperBound)
        {
            availableUpperBound = playerMoney;
        }

        if (success)
        {
            return new Result(
                Success: success,
                Selector: new SelectStartGameMoneyResponse(
                    LowerBound: (int)lowerBound,
                    UpperBound: (int)upperBound,
                    RecommendedValue: (int)recommendedValue,
                    AvailableUpperBound: (int)availableUpperBound,
                    RoomId: roomId),
                ErrorMessage: null);
        }

        const string errMsg = "Your money is not enough to play with such bets.\nPlease reduce your bid.";

        return new Result(Success: success, Selector: null, ErrorMessage: errMsg);
    }
}
