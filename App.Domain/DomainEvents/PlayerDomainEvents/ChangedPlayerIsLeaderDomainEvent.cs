using App.Domain.Primitives;

namespace App.Domain.DomainEvents.PlayerDomainEvents;

public sealed record ChangedPlayerIsLeaderDomainEvent(
    Guid RoomId,
    Guid PlayerId,
    bool IsLeader) : DomainEvent;