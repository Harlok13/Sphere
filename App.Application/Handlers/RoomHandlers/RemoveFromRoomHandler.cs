using App.Application.Repositories;
using App.Application.Repositories.RoomRepository;
using App.Application.Repositories.UnitOfWork;
using App.SignalR.Commands.RoomCommands;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.RoomHandlers;

public class RemoveFromRoomHandler : ICommandHandler<RemoveFromRoomCommand, bool>
{
    private readonly ILogger<RemoveFromRoomHandler> _logger;
    private readonly IPlayerRepository _playerRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IPlayerInfoRepository _playerInfoRepository;
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;

    public RemoveFromRoomHandler(
        ILogger<RemoveFromRoomHandler> logger,
        IPlayerRepository playerRepository,
        IRoomRepository roomRepository,
        IAppUnitOfWork unitOfWork,
        IHubContext<GlobalHub, IGlobalHub> hubContext,
        IPlayerInfoRepository playerInfoRepository)
    {
        _logger = logger;
        _playerRepository = playerRepository;
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
        _hubContext = hubContext;
        _playerInfoRepository = playerInfoRepository;
    }

    public async ValueTask<bool> Handle(RemoveFromRoomCommand command, CancellationToken cT)
    {
        command.Request.Deconstruct(out Guid roomId, out Guid playerId);
        var connectionId = command.ConnectionId;

        if (_roomRepository is RoomRepositoryNotifyDecorator rep)
        {
            rep.NotifyRemoveRoom += RemoveRoomAsync;
        }
        
        var playerInfo = await _unitOfWork.PlayerInfoRepository.GetPlayerInfoByIdAsync(playerId, cT);
        var player = await _unitOfWork.PlayerRepository.GetPlayerByIdAsNoTrackingAsync(playerId, cT);  // TODO: as not tracking
        playerInfo.IncrementMoney(player.Money, connectionId);

        var room = await _unitOfWork.RoomRepository.GetRoomByIdAsync(roomId, cT);
        room.RemovePlayer(playerId, connectionId);
        
        if (room.PlayersInRoom > 0) room.SetNewRoomLeader();
        else await _roomRepository.RemoveRoomAsync(roomId, cT);

        return await _unitOfWork.SaveChangesAsync(cT);
    }

    private async Task RemoveRoomAsync(RoomRepositoryNotifyDecorator.RemoveRoomEventArgs e, CancellationToken cT)
    {
        _logger.LogInformation("Receive remove room in event.");
        await _hubContext.Clients.All.ReceiveAll_RemovedRoom(e.RoomId, cT);
    }
}