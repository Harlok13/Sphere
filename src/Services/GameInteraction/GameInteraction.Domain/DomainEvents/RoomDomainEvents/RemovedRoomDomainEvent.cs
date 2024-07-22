using Core;

namespace GameInteraction.Domain.DomainEvents.RoomDomainEvents;

public sealed record RemovedRoomDomainEvent(
    Guid RoomId) : DomainEvent;