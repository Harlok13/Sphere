using Core;
using GameInteraction.Domain.Primitives;

namespace GameInteraction.Domain.DomainEvents.RoomDomainEvents;

public sealed record AddedGameHistoryMessageDomainEvent(
    Guid RoomId,
    GameHistoryMessage Message) : DomainEvent;