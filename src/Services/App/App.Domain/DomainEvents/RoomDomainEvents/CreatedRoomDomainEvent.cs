using App.Domain.Entities.RoomEntity;
using Core;

namespace App.Domain.DomainEvents.RoomDomainEvents;

public sealed record CreatedRoomDomainEvent(Room Room) : DomainEvent;