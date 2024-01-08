using Mediator;

namespace App.SignalR.Events;

public sealed record UsersNotificationEvent(
    string NotificationText,
    IEnumerable<Guid> TargetIds) : INotification;