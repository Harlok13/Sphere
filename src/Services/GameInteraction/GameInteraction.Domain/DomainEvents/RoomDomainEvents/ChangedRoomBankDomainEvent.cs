using Core;

namespace GameInteraction.Domain.DomainEvents.RoomDomainEvents;

public sealed record ChangedRoomBankDomainEvent(
    Guid RoomId,
    int Bank) : DomainEvent;