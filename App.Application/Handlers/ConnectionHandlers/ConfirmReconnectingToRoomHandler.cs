using App.Application.Repositories.UnitOfWork;
using App.Contracts.Mapper;
using App.Contracts.Responses;
using App.Domain.Entities;
using App.SignalR.Commands.ConnectionCommands;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.ConnectionHandlers;

public class ConfirmReconnectingToRoomHandler : ICommandHandler<ConfirmReconnectingToRoomCommand, bool>
{
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly ILogger<ConfirmReconnectingToRoomHandler> _logger;
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly IPublisher _publisher;

    public ConfirmReconnectingToRoomHandler(
        IAppUnitOfWork unitOfWork,
        ILogger<ConfirmReconnectingToRoomHandler> logger,
        IHubContext<GlobalHub, IGlobalHub> hubContext,
        IPublisher publisher)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _hubContext = hubContext;
        _publisher = publisher;
    }
    
    public async ValueTask<bool> Handle(ConfirmReconnectingToRoomCommand command, CancellationToken cT)
    {
        command.Deconstruct(out IUser user);
        var authUser = user as AuthUser;

        var player = await _unitOfWork.PlayerRepository.GetPlayerByIdAsync(authUser.Id, cT);
        player.SetOnline(true, authUser.ConnectionId);

        /* save the changes to send the correct data. can be done through room, but why? */
        var saveChangesResult = await _unitOfWork.SaveChangesAsync(cT);
        if (!saveChangesResult) return false;
        
        var room = await _unitOfWork.RoomRepository.GetRoomByIdAsNoTrackingAsync(player.RoomId, cT);

        var playerDto = PlayerMapper.MapPlayerToPlayerDto(player);
        var initRoomDataDto = RoomMapper.MapRoomToInitRoomDataDto(room);
        var playersDto = PlayerMapper.MapManyPlayersToManyPlayersDto(room.Players);

        var response = new ReconnectingInitRoomDataResponse(
            Player: playerDto,
            InitRoomData: initRoomDataDto,
            Players: playersDto);
        
        await _hubContext.Clients.Client(player.ConnectionId).ReceiveClient_ReconnectingInitRoomData(response, cT);
        _logger.LogInformation("");

        return saveChangesResult;
    }
}