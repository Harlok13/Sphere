using App.Domain.Primitives;

namespace App.Domain.DomainEvents.PlayerDomainEvents;

public sealed record ChangedPlayerMoneyDomainEvent(
    int Money,
    string ConnectionId,
    Guid RoomId,
    Guid PlayerId) : DomainEvent;