using Core;
using GameInteraction.Domain.Entities;

namespace GameInteraction.Domain.DomainEvents.PlayerDomainEvents;

public sealed record AddedCardDomainEvent(
    Card Card,
    int DelayMs,
    Guid RoomId,
    Guid PlayerId) : DomainEvent;