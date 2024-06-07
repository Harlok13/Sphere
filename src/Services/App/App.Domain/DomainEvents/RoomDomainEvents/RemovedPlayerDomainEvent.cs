using Core;

namespace App.Domain.DomainEvents.RoomDomainEvents;

public sealed record RemovedPlayerDomainEvent(
    Guid RoomId,
    Guid PlayerId,
    string ConnectionId,
    int PlayersInRoom) : DomainEvent;
