using App.Domain.Primitives;

namespace App.Domain.DomainEvents.PlayerDomainEvents;

public sealed record ChangedPlayerInGameDomainEvent(
    Guid RoomId,
    Guid PlayerId,
    bool InGame) : DomainEvent;