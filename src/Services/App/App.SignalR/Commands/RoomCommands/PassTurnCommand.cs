using Mediator;

namespace App.SignalR.Commands.RoomCommands;

public sealed record PassTurnCommand(Guid RoomId) : ICommand<bool>;