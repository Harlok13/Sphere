using Core;

namespace GameInteraction.Domain.DomainEvents.PlayerDomainEvents;

public sealed record ChangedPlayerMoneyDomainEvent(
    int Money,
    Guid RoomId,
    Guid PlayerId) : DomainEvent;