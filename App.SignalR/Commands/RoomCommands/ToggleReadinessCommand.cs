using Mediator;

namespace App.SignalR.Commands.RoomCommands;

public sealed record ToggleReadinessCommand(
    Guid RoomId,
    Guid PlayerId,
    string ConnectionId) : ICommand<bool>;