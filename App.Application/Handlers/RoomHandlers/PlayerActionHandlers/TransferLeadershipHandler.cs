using App.Application.Extensions;
using App.Application.Repositories.UnitOfWork;
using App.Contracts.Data;
using App.Domain.Entities.RoomEntity;
using App.SignalR.Commands.RoomCommands.PlayerActionCommands;
using App.SignalR.Events;
using Mediator;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.RoomHandlers.PlayerActionHandlers;

public class TransferLeadershipHandler : ICommandHandler<TransferLeadershipCommand, bool>
{
    private readonly ILogger<TransferLeadershipHandler> _logger;
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher;

    public TransferLeadershipHandler(
        ILogger<TransferLeadershipHandler> logger,
        IAppUnitOfWork unitOfWork,
        IPublisher publisher)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }
    
    /* the return value is responsible for closing the modal window. true - close the window, false - leave it open */
    public async ValueTask<bool> Handle(TransferLeadershipCommand command, CancellationToken cT)
    {
        command.Request.Deconstruct(out Guid senderId, out Guid receiverId, out Guid roomId);

        var senderResult = await _unitOfWork.PlayerRepository.GetPlayerByIdAsNoTrackingAsync(senderId, cT);
        if (!senderResult.TryFromResult(out PlayerDto? senderNoTrack, out var errors))
        {
            foreach (var error in errors)
                _logger.LogError(error.Message);
            
            await _publisher.Publish(new UserNotificationEvent(
                    NotificationText: NotificationMessages.SomethingWentWrong(),
                    TargetId: senderId),
                cT);
            
            return false;
        }
        
        if (!senderNoTrack!.IsLeader)
        {
            await _publisher.Publish(new UserNotificationEvent(
                    NotificationText: NotificationMessages.TransferLeadership.NotLeader(),
                    TargetId: senderNoTrack.Id),
                cT);
            // const string notificationText = "You are not a leader to pass on leadership.";
            // var response = new NotificationResponse(Guid.NewGuid(), notificationText);
            //
            // await _hubContext.Clients
            //     .Client(transmittingPlayer.ConnectionId)
            //     .ReceiveClient_Notification(response, cT);
            // _logger.LogInformation("");

            return false;
        }
        
        var roomResult = await _unitOfWork.RoomRepository.GetByIdAsync(roomId, cT);
        if (!roomResult.TryFromResult(out Room? room, out var roomErrors))
        {
            foreach(var error in roomErrors) _logger.LogError(error.Message);

            await _publisher.Publish(new UserNotificationEvent(
                    NotificationText: NotificationMessages.SomethingWentWrong(),
                    TargetId: senderId),
                cT);

            return false;
        }
        
        var transferResult = room!.TransferLeadership(senderId, receiverId);
        if (!transferResult.TryFromResult(out Room.TransferLeadershipDto? data, out var transferErrors))
        {
            foreach (var error in transferErrors) _logger.LogError(error.Message);

            await _publisher.Publish(new UserNotificationEvent(
                    NotificationText: NotificationMessages.SomethingWentWrong(),
                    TargetId: senderId),
                cT);

            return false;
        }
        // var receiver = room.Players.Single(p => p.Id == receiverId);
        // receiver.SetIsLeader(true);
        //
        // var sender = room.Players.Single(p => p.Id == senderId);
        // sender.SetIsLeader(false);

        await _publisher.Publish(new UserNotificationEvent(
                NotificationText: NotificationMessages.TransferLeadership.SuccessTransfer(data!.ReceiverName),
                TargetId: senderId),
            cT);

        await _publisher.Publish(new UserNotificationEvent(
                NotificationText: NotificationMessages.TransferLeadership.ReceiveLeadership(data.SenderName),
                TargetId: receiverId),
            cT);
        
        // var notifMsg = $"You have given leadership to the player \"{receiver.PlayerName}\".";
        // var notifResponse = new NotificationResponse(Guid.NewGuid(), notifMsg);
        // await _hubContext.Clients.Client(transmittingPlayer.ConnectionId).ReceiveClient_Notification(notifResponse, cT);
        // _logger.LogInformation("");
        //
        // var nMsg = $"The player \"{transmittingPlayer.PlayerName}\" gave you the lead.";
        // var nResponse = new NotificationResponse(Guid.NewGuid(), nMsg);
        // await _hubContext.Clients.Client(receiver.ConnectionId).ReceiveClient_Notification(nResponse, cT);
        // _logger.LogInformation("");

        return await _unitOfWork.SaveChangesAsync(cT);
    }
}