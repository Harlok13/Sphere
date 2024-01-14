using Mediator;

namespace App.SignalR.Commands.RoomCommands;

public sealed record EndCycleCommand(
    Guid RoomId) : ICommand<bool>;