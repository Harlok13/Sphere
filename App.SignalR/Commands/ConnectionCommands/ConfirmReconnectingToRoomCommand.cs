using App.Contracts.Requests.ConnectionRequests;
using App.Domain.Entities;
using Mediator;

namespace App.SignalR.Commands.ConnectionCommands;

public sealed record ConfirmReconnectingToRoomCommand(
    AuthUser AuthUser) : ICommand<bool>;