using App.Contracts.Requests;
using Mediator;

namespace App.SignalR.Commands.RoomCommands;

public sealed record ToggleReadinessCommand(
    ToggleReadinessRequest Request) : ICommand<bool>;