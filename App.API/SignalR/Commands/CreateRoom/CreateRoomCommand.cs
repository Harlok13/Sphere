using App.Contracts.Requests;
using Mediator;

namespace Sphere.SignalR.Commands.CreateRoom;

public sealed record CreateRoomCommand(
    RoomRequest RoomRequest,
    Guid UserId,
    string UserName,
    string ConnectionId,
    bool IsLeader = true
) : ICommand<bool>;