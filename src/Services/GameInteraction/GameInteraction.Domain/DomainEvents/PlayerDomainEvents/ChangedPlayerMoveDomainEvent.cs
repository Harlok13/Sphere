using Core;

namespace GameInteraction.Domain.DomainEvents.PlayerDomainEvents;

public sealed record ChangedPlayerMoveDomainEvent(
    bool Move,
    Guid PlayerId) : DomainEvent;