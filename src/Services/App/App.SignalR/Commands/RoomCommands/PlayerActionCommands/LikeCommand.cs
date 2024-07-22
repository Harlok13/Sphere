using Mediator;

namespace App.SignalR.Commands.RoomCommands.PlayerActionCommands;

public sealed record LikeCommand(
    ) : ICommand<bool>;