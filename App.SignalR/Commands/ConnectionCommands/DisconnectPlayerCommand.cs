using App.Domain.Entities;
using Mediator;

namespace App.SignalR.Commands.ConnectionCommands;

public sealed record DisconnectPlayerCommand(
    AuthUser AuthUser) : ICommand<bool>;