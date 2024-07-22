using App.Contracts.Requests;
using Mediator;

namespace App.SignalR.Commands.RoomCommands;

public sealed record StartGameCommand(
    StartGameRequest Request) : ICommand<bool>;