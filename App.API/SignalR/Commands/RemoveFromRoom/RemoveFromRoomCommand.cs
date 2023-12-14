using Mediator;

namespace Sphere.SignalR.Commands.RemoveFromRoom;

public sealed record RemoveFromRoomCommand(
    Guid RoomId,
    Guid PlayerId) : ICommand<bool>;