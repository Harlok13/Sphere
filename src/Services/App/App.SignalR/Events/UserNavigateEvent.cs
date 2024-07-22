using App.Contracts.Enums;
using Mediator;

namespace App.SignalR.Events;

public sealed record UserNavigateEvent(
    Guid TargetId,
    NavigateEnum Navigate) : INotification;