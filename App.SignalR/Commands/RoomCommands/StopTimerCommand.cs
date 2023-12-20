using Mediator;

namespace App.SignalR.Commands.RoomCommands;

public sealed record StopTimerCommand(
    Guid RoomId,
    Guid PlayerId,
    string ConnectionId,
    CancellationTokenSource Cts) : ICommand<bool>;