using App.Domain.Primitives;

namespace App.Domain.DomainEvents.RoomDomainEvents;

public sealed record AddedGameHistoryMessageDomainEvent(
    Guid RoomId,
    GameHistoryMessage Message) : DomainEvent;