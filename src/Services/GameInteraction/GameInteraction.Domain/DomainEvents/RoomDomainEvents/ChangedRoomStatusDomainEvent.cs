using Core;
using GameInteraction.Domain.Enums;

namespace GameInteraction.Domain.DomainEvents.RoomDomainEvents;

public sealed record ChangedRoomStatusDomainEvent(
    Guid RoomId,
    ERoomStatus RoomStatus) : DomainEvent;