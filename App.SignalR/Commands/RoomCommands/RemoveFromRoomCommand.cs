using Mediator;

namespace App.SignalR.Commands.RoomCommands;

public sealed record RemoveFromRoomCommand(
    Guid RoomId,
    Guid PlayerId,
    string ConnectionId) : ICommand<bool>;