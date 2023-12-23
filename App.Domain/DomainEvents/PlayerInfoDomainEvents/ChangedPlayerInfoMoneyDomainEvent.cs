using App.Domain.Primitives;

namespace App.Domain.DomainEvents.PlayerInfoDomainEvents;

public sealed record ChangedPlayerInfoMoneyDomainEvent(
    int Money,
    string ConnectionId) : DomainEvent;