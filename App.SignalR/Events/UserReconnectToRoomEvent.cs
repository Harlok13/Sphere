using Mediator;

namespace App.SignalR.Events;

public sealed record UserReconnectToRoomEvent(
    Guid RoomId,
    Guid PlayerId) : INotification;