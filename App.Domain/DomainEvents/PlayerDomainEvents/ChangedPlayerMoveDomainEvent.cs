using App.Domain.Primitives;

namespace App.Domain.DomainEvents.PlayerDomainEvents;

public sealed record ChangedPlayerMoveDomainEvent(
    bool Move,
    string ConnectionId) : DomainEvent;