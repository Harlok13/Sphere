using App.Domain.Primitives;

namespace App.Domain.DomainEvents.PlayerDomainEvents;

public sealed record ChangedPlayerMoneyDomainEvent(
    int Money,
    Guid RoomId,
    Guid PlayerId) : DomainEvent;