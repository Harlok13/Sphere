using App.Domain.DomainEvents.NotificationDomainEvent;
using App.SignalR.Events;
using Mediator;
using Microsoft.Extensions.Logging;

namespace App.SignalR.DomainEventHandlers.NotificationDomainEventHandlers;

public class BothSideNotifiedDomainEventHandler : INotificationHandler<BothSideNotifiedDomainEvent>
{
    private readonly IPublisher _publisher;
    private readonly ILogger<BothSideNotifiedDomainEventHandler> _logger;

    public BothSideNotifiedDomainEventHandler(
        ILogger<BothSideNotifiedDomainEventHandler> logger,
        IPublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async ValueTask Handle(BothSideNotifiedDomainEvent notification, CancellationToken cT)
    {
        notification.Deconstruct(
            out Guid mainSideId, 
            out Guid secondSideId,
            out string notificationForMainSide,
            out string notificationForSecondSide);
        
        _logger.LogInformation(
            "Send a notification to \"{MainSideId}\" and \"{SecondSideId}\".",
            mainSideId,
            secondSideId);
        
        await _publisher.Publish(new UserNotificationEvent(
                NotificationText: notificationForMainSide,
                TargetId: mainSideId),
            cT);

        await _publisher.Publish(new UserNotificationEvent(
                NotificationText: notificationForSecondSide,
                TargetId: secondSideId),
            cT);
    }
}