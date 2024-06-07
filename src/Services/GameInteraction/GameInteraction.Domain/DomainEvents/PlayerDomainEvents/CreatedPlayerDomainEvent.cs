using Core;
using GameInteraction.Domain.Entities.PlayerEntity;
using GameInteraction.Domain.Entities.RoomEntity;

namespace GameInteraction.Domain.DomainEvents.PlayerDomainEvents;

public record CreatedPlayerDomainEvent(
    Player Player,
    Room Room) : DomainEvent;