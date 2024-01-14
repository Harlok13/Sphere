using Mediator;

namespace App.SignalR.Commands.RoomCommands;

public sealed record EndGameCommand(
    Guid RoomId) : ICommand<bool>;