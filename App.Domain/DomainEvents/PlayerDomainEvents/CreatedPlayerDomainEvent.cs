using App.Domain.Entities.PlayerEntity;
using App.Domain.Entities.RoomEntity;
using App.Domain.Primitives;

namespace App.Domain.DomainEvents.PlayerDomainEvents;

public record CreatedPlayerDomainEvent(
    Player Player,
    Room Room) : DomainEvent;