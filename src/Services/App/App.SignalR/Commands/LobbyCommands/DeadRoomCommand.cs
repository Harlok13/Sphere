using Mediator;

namespace App.SignalR.Commands.LobbyCommands;

public sealed record DeadRoomCommand(
    CancellationTokenSource Cts) : ICommand<bool>;