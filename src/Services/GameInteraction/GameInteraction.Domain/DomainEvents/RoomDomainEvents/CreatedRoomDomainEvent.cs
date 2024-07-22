using Core;
using GameInteraction.Domain.Entities.RoomEntity;

namespace GameInteraction.Domain.DomainEvents.RoomDomainEvents;

public sealed record CreatedRoomDomainEvent(Room Room) : DomainEvent;