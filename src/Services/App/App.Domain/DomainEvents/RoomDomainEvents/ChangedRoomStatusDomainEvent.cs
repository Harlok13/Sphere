using App.Domain.Enums;
using Core;

namespace App.Domain.DomainEvents.RoomDomainEvents;

public sealed record ChangedRoomStatusDomainEvent(
    Guid RoomId,
    RoomStatus RoomStatus) : DomainEvent;