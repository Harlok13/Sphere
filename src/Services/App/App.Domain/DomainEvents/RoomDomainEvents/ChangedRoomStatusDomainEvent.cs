using App.Domain.Enums;
using App.Domain.Primitives;

namespace App.Domain.DomainEvents.RoomDomainEvents;

public sealed record ChangedRoomStatusDomainEvent(
    Guid RoomId,
    RoomStatus RoomStatus) : DomainEvent;