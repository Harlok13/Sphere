using Core;

namespace GameInteraction.Domain.DomainEvents.RoomDomainEvents;

public sealed record ChangedRoomRoomNameDomainEvent(
    Guid RoomId,
    string RoomName) : DomainEvent;