using Mediator;

namespace App.SignalR.Commands.RoomCommands;

public sealed record StartGameCommand(
    Guid RoomId,
    Guid PlayerId) : ICommand<bool>;