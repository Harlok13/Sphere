using App.Domain.Entities;
using Core;

namespace App.Domain.DomainEvents.PlayerDomainEvents;

public sealed record AddedCardDomainEvent(
    Card Card,
    int DelayMs,
    Guid RoomId,
    Guid PlayerId) : DomainEvent;