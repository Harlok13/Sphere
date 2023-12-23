using App.Domain.Primitives;

namespace App.Domain.DomainEvents.RoomDomainEvents;

public sealed record ChangedRoomRoomNameDomainEvent(
    Guid RoomId,
    string RoomName) : DomainEvent;