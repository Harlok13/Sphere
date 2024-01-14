using Mediator;

namespace App.SignalR.Events;

public sealed record UserNotificationEvent(
    string NotificationText,
    Guid TargetId) : INotification;