using App.Domain.Entities;
using App.Domain.Primitives;

namespace App.Domain.DomainEvents.PlayerDomainEvents;

public sealed record AddedCardDomainEvent(
    Card Card,
    int DelayMs,
    Guid RoomId,
    Guid PlayerId,
    string ConnectionId) : DomainEvent;