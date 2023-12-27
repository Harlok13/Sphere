using App.Domain.Entities;
using Mediator;

namespace App.SignalR.Commands.ConnectionCommands;

public sealed record DisconnectPlayerCommand(
    IUser AuthUser) : ICommand<bool>;