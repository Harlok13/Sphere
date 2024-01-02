using App.Application.Extensions;
using App.Application.Repositories.UnitOfWork;
using App.Domain.DomainResults;
using App.Domain.Entities.PlayerInfoEntity;
using App.Domain.Entities.RoomEntity;
using App.Domain.Shared;
using App.SignalR.Commands.LobbyCommands;
using App.SignalR.Events;
using Mediator;
using Microsoft.Extensions.Logging;

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
        
        var roomResult = await _unitOfWork.RoomRepository.GetByIdAsync(roomId, cT);
        if (!roomResult.TryFromResult(out Room? room, out var roomErrors))
        {
            return await SendSomethingWentWrongNotification(roomErrors, connectionId, cT);
        }

        var canPlayerJoinResult = room!.CanPlayerJoin(playerId);
        if (canPlayerJoinResult is DomainFailure canPlayerJoinFailure)
        {
            await _publisher.Publish(new ClientNotificationEvent(
                    NotificationText: canPlayerJoinFailure.Reason,
                    TargetConnectionId: connectionId),
                cT);

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
        if (joinResult is DomainFailure joinFailure)
        {
            await _publisher.Publish(new UserNotificationEvent(
                    NotificationText: joinFailure.Reason,
                    TargetId: playerId),
                cT);

            return false;
        }

        var saveChangesResult = await _unitOfWork.SaveChangesAsync(cT);
        if (!saveChangesResult)
        {
            return await SendSomethingWentWrongNotification(null, connectionId, cT);
        }

        return saveChangesResult;
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
