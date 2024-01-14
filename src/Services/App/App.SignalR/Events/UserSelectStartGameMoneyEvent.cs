using App.Contracts.Responses;
using Mediator;

namespace App.SignalR.Events;

public sealed record UserSelectStartGameMoneyEvent(
    Guid PlayerId,
    SelectStartGameMoneyResponse Response) : INotification;