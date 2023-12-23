using App.Contracts.Requests;
using Mediator;

namespace App.SignalR.Commands.LobbyCommands;

public sealed record JoinToRoomCommand(
    JoinToRoomRequest Request,
    string ConnectionId) : ICommand<bool>;