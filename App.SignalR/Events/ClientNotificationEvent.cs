using Mediator;

namespace App.SignalR.Events;

public sealed record ClientNotificationEvent(
    string NotificationText,
    string TargetConnectionId) : INotification;