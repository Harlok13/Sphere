using App.Domain.Primitives;

namespace App.Domain.DomainEvents.RoomDomainEvents;

public sealed record RemovedRoomDomainEvent(
    Guid RoomId) : DomainEvent;