using App.Contracts.Requests;
using Mediator;

namespace App.SignalR.Commands.LobbyCommands;

public sealed record CreateRoomCommand(
    CreateRoomRequest Request,
    string ConnectionId
) : ICommand<bool>;