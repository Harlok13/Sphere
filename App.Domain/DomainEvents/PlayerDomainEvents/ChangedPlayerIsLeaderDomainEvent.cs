using App.Domain.Primitives;

namespace App.Domain.DomainEvents.PlayerDomainEvents;

public sealed record ChangedPlayerIsLeaderDomainEvent(
    Guid RoomId,
    Guid PlayerId,
    int PlayersInRoom,
    bool IsLeader,
    string ConnectionId) : DomainEvent;