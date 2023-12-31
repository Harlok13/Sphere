using App.Domain.Primitives;

namespace App.Domain.DomainEvents.RoomDomainEvents;

public sealed record AddedKickedPlayerDomainEvent(
    string InitiatorConnectionId,
    string KickedPlayerConnectionId) : DomainEvent;