using App.Domain.DomainEvents.RoomDomainEvents;
using App.SignalR.Events;
using Mediator;
using Microsoft.Extensions.Logging;

namespace App.SignalR.DomainEventHandlers.RoomDomainEventHandlers;

public class AddedKickedPlayerDomainEventHandler : INotificationHandler<AddedKickedPlayerDomainEvent>
{
    private readonly IPublisher _publisher;
    private readonly ILogger<AddedKickedPlayerDomainEventHandler> _logger;

    public AddedKickedPlayerDomainEventHandler(
        ILogger<AddedKickedPlayerDomainEventHandler> logger,
        IPublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async ValueTask Handle(AddedKickedPlayerDomainEvent notification, CancellationToken cT)
    {
        notification.Deconstruct(
            out string initiatorConnectionId, 
            out string kickedPlayerConnectionId,
            out string notificationForInitiator,
            out string notificationForKickedPlayer);
        
        _logger.LogInformation(
            "Send a notification to \"{InitiatorConnId}\" and \"{KickedConnId}\".",
            initiatorConnectionId,
            kickedPlayerConnectionId);
        
        await _publisher.Publish(new ClientNotificationEvent(
                NotificationText: notificationForInitiator,
                TargetConnectionId: initiatorConnectionId),
            cT);

        await _publisher.Publish(new ClientNotificationEvent(
                NotificationText: notificationForKickedPlayer,
                TargetConnectionId: kickedPlayerConnectionId),
            cT);
    }
}