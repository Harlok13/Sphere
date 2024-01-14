using App.Domain.Entities.RoomEntity;
using App.Domain.Primitives;

namespace App.Domain.DomainEvents.RoomDomainEvents;

public sealed record CreatedRoomDomainEvent(Room Room) : DomainEvent;