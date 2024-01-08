using Mediator;

namespace App.SignalR.Commands.RoomCommands.GameActionCommands;

public sealed record BetCommand() : ICommand<bool>;