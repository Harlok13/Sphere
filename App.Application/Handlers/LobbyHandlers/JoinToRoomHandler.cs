using App.Application.Extensions;
using App.Application.Repositories.UnitOfWork;
using App.Domain.Entities.PlayerInfoEntity;
using App.Domain.Entities.RoomEntity;
using App.Domain.Shared;
using App.SignalR.Commands.LobbyCommands;
using App.SignalR.Events;
using Mediator;
using Microsoft.Extensions.Logging;
using Player = App.Domain.Entities.PlayerEntity.Player;

namespace App.Application.Handlers.LobbyHandlers;

public class JoinToRoomHandler : ICommandHandler<JoinToRoomCommand, bool>
{
    private readonly ILogger<JoinToRoomHandler> _logger;
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher;

    public JoinToRoomHandler(
        ILogger<JoinToRoomHandler> logger,
        IAppUnitOfWork unitOfWork, 
        IPublisher publisher)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }
    
    public async ValueTask<bool> Handle(JoinToRoomCommand command, CancellationToken cT)
    {
        command.Request.Deconstruct(
            out Guid roomId, out Guid playerId, out int selectedStartMoney);
        var connectionId = command.ConnectionId;
        
        // TODO: check player is kicked
        
        var roomResult = await _unitOfWork.RoomRepository.GetByIdAsync(roomId, cT);
        if (!roomResult.TryFromResult(out Room? room, out var roomErrors))
        {
            return await SendSomethingWentWrongNotification(roomErrors, connectionId, cT);
        }

        // if (room!.Players.Count == room.RoomSize)
        // {
        //     // TODO: room is full message (_hubContext.Clients.Client(player.ConnectionId).ReceiveOwn_RoomIsFullNotification());
        //     return false;
        // }

        var canPlayerJoinResult = room!.CanPlayerJoin(playerId);
        if (canPlayerJoinResult is Room.CanPlayerJoinFailure)
        {
            if (canPlayerJoinResult is Room.RoomIsFull roomIsFullResult)
            {
                await _publisher.Publish(new ClientNotificationEvent(
                        NotificationText: roomIsFullResult.Reason,
                        TargetConnectionId: connectionId),
                    cT);
            }
            else if (canPlayerJoinResult is Room.PlayerWasKicked playerWasKickedResult)
            {
                await _publisher.Publish(new ClientNotificationEvent(
                        NotificationText: playerWasKickedResult.Reason,
                        TargetConnectionId: connectionId),
                    cT);
            }
        }
        
        var playerInfoResult = await _unitOfWork.PlayerInfoRepository.GetPlayerInfoByIdAsync(playerId, cT);
        if (!playerInfoResult.TryFromResult(out PlayerInfo? playerInfo, out var playerInfoErrors))
        {
            return await SendSomethingWentWrongNotification(playerInfoErrors, connectionId, cT);
        }
        
        var joinToRoomResult = playerInfo!.JoinToRoom(selectedStartMoney);
        if (!joinToRoomResult.TryFromResult(out PlayerInfo.JoinToRoomDto? data, out var joinToRoomErrors))
        {
            return await SendSomethingWentWrongNotification(joinToRoomErrors, connectionId, cT);
        }
        
        var joinResult = room.JoinToRoom(
            playerId: playerId,
            playerName: data!.PlayerName, 
            data.Money,
            connectionId: connectionId);
        if (joinResult.IsFailure)
        {
            return await SendSomethingWentWrongNotification(joinResult.Errors!, connectionId, cT);
        }

        var saveChangesResult = await _unitOfWork.SaveChangesAsync(cT);
        if (!saveChangesResult)
        {
            return await SendSomethingWentWrongNotification(null, connectionId, cT);
        }

        return saveChangesResult;


        // var playerInfo = await _playerInfoRepository.GetPlayerInfoByIdAsync(playerId, cT);
        //
        // playerInfo.DecrementMoney(selectedStartMoney);
        //
        // var isLeader = room.Players.Count < 1;
        // var player = Player.Create(
        //     id: playerId,
        //     playerName: playerInfo.PlayerName,
        //     roomId: roomId,
        //     money: selectedStartMoney,
        //     connectionId: connectionId,
        //     isLeader: isLeader,
        //     room: room);
        // _logger.LogInformation($"room is null? {player.Room is null}");
        // room.AddNewPlayer(player);
        //
        // return await _unitOfWork.SaveChangesAsync(cT);
    }
    
    private async ValueTask<bool> SendSomethingWentWrongNotification(
        IEnumerable<Error>? errors,
        string targetConnectionId,
        CancellationToken cT)
    {
        if (errors is not null)
            foreach (var error in errors) _logger.LogError(error.Message);
            
        await _publisher.Publish(new ClientNotificationEvent(
                NotificationText: NotificationMessages.SomethingWentWrong(),
                TargetConnectionId: targetConnectionId),
            cT);
            
        return false;
    }
}
