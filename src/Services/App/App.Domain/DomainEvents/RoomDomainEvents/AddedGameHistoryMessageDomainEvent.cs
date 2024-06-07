using App.Domain.Primitives;
using Core;

namespace App.Domain.DomainEvents.RoomDomainEvents;

public sealed record AddedGameHistoryMessageDomainEvent(
    Guid RoomId,
    GameHistoryMessage Message) : DomainEvent;