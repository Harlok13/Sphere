using Core;

namespace GameInteraction.Domain.DomainEvents.PlayerDomainEvents;

public sealed record ChangedPlayerIsLeaderDomainEvent(
    Guid RoomId,
    Guid PlayerId,
    bool IsLeader) : DomainEvent;