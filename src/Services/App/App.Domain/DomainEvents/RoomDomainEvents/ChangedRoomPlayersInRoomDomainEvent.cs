using Core;

namespace App.Domain.DomainEvents.RoomDomainEvents;

public sealed record ChangedRoomPlayersInRoomDomainEvent(
    Guid RoomId,
    int PlayersInRoom) : DomainEvent;