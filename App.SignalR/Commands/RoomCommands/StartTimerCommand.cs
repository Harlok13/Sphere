using Mediator;

namespace App.SignalR.Commands.RoomCommands;

public sealed record StartTimerCommand(
    Guid RoomId,
    Guid PlayerId,
    string ConnectionId,
    CancellationTokenSource Cts) : ICommand<bool>;