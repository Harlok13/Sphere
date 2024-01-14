using App.Domain.Entities.PlayerEntity;
using App.Domain.Entities.RoomEntity;
using App.Domain.Primitives;

namespace App.Domain.DomainEvents.RoomDomainEvents;

public record AddedPlayerDomainEvent(
    Player Player,
    Room Room) : DomainEvent;