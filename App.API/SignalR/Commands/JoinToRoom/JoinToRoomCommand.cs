using Mediator;

namespace Sphere.SignalR.Commands.JoinToRoom;

public sealed record JoinToRoomCommand(
    Guid RoomId,
    Guid PlayerId,
    string PlayerName, 
    string ConnectionId,
    bool IsLeader = false) : ICommand<bool>;