using Core;

namespace App.Domain.DomainEvents.PlayerDomainEvents;

public sealed record ChangedPlayerOnlineDomainEvent(
    bool Online,
    Guid RoomId,
    Guid PlayerId,
    string ConnectionId) : DomainEvent;