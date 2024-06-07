using Core;

namespace App.Domain.DomainEvents.PlayerDomainEvents;

public sealed record ChangedPlayerReadinessDomainEvent(
    bool Readiness,
    string ConnectionId,
    Guid RoomId,
    Guid PlayerId) : DomainEvent;