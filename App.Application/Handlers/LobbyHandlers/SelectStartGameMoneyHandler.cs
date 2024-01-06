using App.Application.Extensions;
using App.Application.Messages;
using App.Application.Repositories.UnitOfWork;
using App.Contracts.Requests;
using App.Contracts.Responses;
using App.SignalR.Commands.LobbyCommands;
using App.SignalR.Events;
using Mediator;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.LobbyHandlers;

public class SelectStartGameMoneyHandler : ICommandHandler<SelectStartGameMoneyCommand, bool>
{
    private readonly ILogger<SelectStartGameMoneyHandler> _logger;
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher;

    private sealed record Result(bool Success, SelectStartGameMoneyResponse? Selector, string? ErrorMessage);

    public SelectStartGameMoneyHandler(
        ILogger<SelectStartGameMoneyHandler> logger,
        IAppUnitOfWork unitOfWork,
        IPublisher publisher)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    public async ValueTask<bool> Handle(SelectStartGameMoneyCommand command, CancellationToken cT)
    {
        command.Request.Deconstruct(out RoomRequest roomRequest, out Guid playerId, out Guid? roomId);

        var playerInfoMoneyResult = await _unitOfWork.PlayerInfoRepository.GetMoneyByIdAsync(playerId, cT);
        if (!playerInfoMoneyResult.TryFromResult(out var playerInfoData, out var errors))
        {
            foreach (var error in errors) _logger.LogError(error.Message);

            await _publisher.Publish(new UserNotificationEvent(
                    NotificationText: NotificationMessages.SomethingWentWrong(),
                    TargetId: playerId),
                cT);

            return false;
        }

        var selectorResult = ComputeSelectStartGameMoney(
            startBid: roomRequest.StartBid,
            minBid: roomRequest.MinBid,
            maxBid: roomRequest.MaxBid,
            playerMoney: playerInfoData!.Money,
            roomId: roomId);

        if (selectorResult.Success)
        {
            await _publisher.Publish(new UserSelectStartGameMoneyEvent(
                    PlayerId: playerId,
                    Response: selectorResult.Selector!),
                cT);

            return true;
        }

        await _publisher.Publish(new UserNotificationEvent(
                NotificationText: selectorResult.ErrorMessage!,
                TargetId: playerId),
            cT);

        return false;
    }

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
        // TODO: finish method
        const string errMsg = "Your money is not enough to play with such bets.\nPlease reduce your bid.";

        return new Result(Success: success, Selector: null, ErrorMessage: errMsg);
    }
}