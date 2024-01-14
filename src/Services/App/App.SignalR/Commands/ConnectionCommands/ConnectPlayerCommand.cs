using App.Domain.Entities;
using Mediator;

namespace App.SignalR.Commands.ConnectionCommands;

public sealed record ConnectPlayerCommand(
    AuthUser AuthUser) : ICommand<bool>;