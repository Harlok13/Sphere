using App.Domain.Entities.PlayerEntity;
using App.Domain.Entities.RoomEntity;
using Core;

namespace App.Domain.DomainEvents.PlayerDomainEvents;

public record CreatedPlayerDomainEvent(
    Player Player,
    Room Room) : DomainEvent;