using App.Domain.Primitives;

namespace App.Domain.DomainEvents.RoomDomainEvents;

public sealed record ChangedRoomBankDomainEvent(
    Guid RoomId,
    int Bank) : DomainEvent;