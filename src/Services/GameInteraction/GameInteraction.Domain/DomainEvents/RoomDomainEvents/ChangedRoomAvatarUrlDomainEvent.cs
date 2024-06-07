using Core;

namespace GameInteraction.Domain.DomainEvents.RoomDomainEvents;

public sealed record ChangedRoomAvatarUrlDomainEvent(
    Guid RoomId,
    string AvatarUrl) : DomainEvent;