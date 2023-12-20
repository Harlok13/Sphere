using App.Domain.Entities.RoomEntity;
using App.Domain.Primitives;

namespace App.Domain.DomainEvents.RoomEvents;

public sealed record CreateRoomEvent(Room Room) : DomainEvent;