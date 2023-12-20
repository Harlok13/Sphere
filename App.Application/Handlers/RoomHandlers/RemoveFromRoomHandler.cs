using App.Application.Mapper;
using App.Application.Repositories;
using App.Application.Repositories.RoomRepository;
using App.Application.Repositories.UnitOfWork;
using App.Domain.Entities.RoomEntity;
using App.SignalR.Commands;
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
        command.Deconstruct(out Guid roomId, out Guid playerId, out string connectionId);

        // _roomRepository is RoomRepositoryNotifyDecorator rep;
        if (_roomRepository is RoomRepositoryNotifyDecorator rep)
        {
            rep.NotifyRemoveRoom += RemoveFromRoom;
        }
        
        var playerInfo = await _playerInfoRepository.GetPlayerInfoByIdAsync(playerId, cT);
        var player = await _playerRepository.GetPlayerByIdAsync(playerId, cT);
        playerInfo.IncrementMoney(player.Money);
        await _hubContext.Clients.Client(connectionId).ReceiveOwn_UpdatedMoney(playerInfo.Money, cT);

        var room = await _roomRepository.GetRoomByIdAsync(roomId, cT);
        room.RemovePlayer(playerId);
        
        // await _playerRepository.RemovePlayerAsync(playerId, cT);

        await _hubContext.Clients.Client(connectionId).ReceiveOwn_RemoveFromRoom(cT);

        if (room.PlayersInRoom > 0)
        {
            var newRoomLeader = room.SetNewRoomLeader();

            var roomInLobbyResponse = RoomMapper.MapRoomToRoomInLobbyResponse(room);
            var newLeaderResponse = PlayerMapper.MapPlayerToPlayerResponse(newRoomLeader);
            
            await _hubContext.Clients.Group(roomId.ToString()).ReceiveGroup_RemovedPlayer(playerId, cT);
            await _hubContext.Clients.Group(roomId.ToString()).ReceiveGroup_NewRoomLeader(newLeaderResponse, cT);  // TODO: receive player id only
            await _hubContext.Clients.Group(roomId.ToString()).ReceiveGroup_NewRoomName(room.RoomName, cT);
            await _hubContext.Clients.All.ReceiveAll_UpdatedRoom(roomInLobbyResponse, cT);
        }
        else
        {
            
            await _roomRepository.RemoveRoomAsync(roomId, cT);
            // await _hubContext.Clients.All.ReceiveAll_RemovedRoom(roomId, cT);
        }

        await _unitOfWork.SaveChangesAsync(cT);
        
        return true;
    }

    private async Task RemoveFromRoom(RoomRepositoryNotifyDecorator.RemoveRoomEventArgs e, CancellationToken cT)
    {
        _logger.LogInformation("Receive remove room in event.");
        await _hubContext.Clients.All.ReceiveAll_RemovedRoom(e.RoomId, cT);
    }
}