using Mediator;

namespace App.SignalR.Commands.RoomCommands;

public sealed record FoldCommand(
    Guid RoomId,
    Guid PlayerId) : ICommand<bool>;