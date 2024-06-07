using Core;

namespace GameInteraction.Domain.DomainEvents.PlayerDomainEvents;

public sealed record ChangedPlayerInGameDomainEvent(
    Guid RoomId,
    Guid PlayerId,
    bool InGame) : DomainEvent;