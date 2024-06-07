using Core;
using GameInteraction.Domain.Entities.PlayerEntity;
using GameInteraction.Domain.Entities.RoomEntity;

namespace GameInteraction.Domain.DomainEvents.RoomDomainEvents;

public record AddedPlayerDomainEvent(
    Player Player,
    Room Room) : DomainEvent;