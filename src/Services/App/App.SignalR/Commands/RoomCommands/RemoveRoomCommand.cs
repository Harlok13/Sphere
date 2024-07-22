using Mediator;

namespace App.SignalR.Commands.RoomCommands;

public sealed record RemoveRoomCommand(
    Guid RoomId) : ICommand<bool>;