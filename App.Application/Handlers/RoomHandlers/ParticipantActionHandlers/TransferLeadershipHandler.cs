using App.Application.Extensions;
using App.Application.Messages;
using App.Application.Repositories.UnitOfWork;
using App.Domain.DomainResults;
using App.Domain.Entities.RoomEntity;
using App.SignalR.Commands.RoomCommands.PlayerActionCommands;
using App.SignalR.Events;
using Mediator;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.RoomHandlers.ParticipantActionHandlers;

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
        if (transferResult is DomainError transferError)
        {
            _logger.LogError(transferError.Reason);

            await _publisher.Publish(new UserNotificationEvent(
                    NotificationText: NotificationMessages.SomethingWentWrong(),
                    TargetId: senderId),
                cT);

            return false;
        }

        if (transferResult is DomainFailure transferFailure)
        {
            await _publisher.Publish(new UserNotificationEvent(
                    NotificationText: transferFailure.Reason,
                    TargetId: senderId),
                cT);

            return false;
        }

        return await _unitOfWork.SaveChangesAsync(cT);
    }
}