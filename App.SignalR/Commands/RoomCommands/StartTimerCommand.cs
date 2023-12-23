using App.Contracts.Requests;
using Mediator;

namespace App.SignalR.Commands.RoomCommands;

public sealed record StartTimerCommand(
    StartTimerRequest Request,
    string ConnectionId,
    CancellationTokenSource Cts) : ICommand<bool>;