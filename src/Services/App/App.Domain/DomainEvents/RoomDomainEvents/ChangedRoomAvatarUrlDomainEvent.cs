using Core;

namespace App.Domain.DomainEvents.RoomDomainEvents;

public sealed record ChangedRoomAvatarUrlDomainEvent(
    Guid RoomId,
    string AvatarUrl) : DomainEvent;