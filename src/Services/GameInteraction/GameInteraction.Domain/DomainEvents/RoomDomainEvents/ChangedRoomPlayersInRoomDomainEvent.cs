using Core;

namespace GameInteraction.Domain.DomainEvents.RoomDomainEvents;

public sealed record ChangedRoomPlayersInRoomDomainEvent(
    Guid RoomId,
    int PlayersInRoom) : DomainEvent;