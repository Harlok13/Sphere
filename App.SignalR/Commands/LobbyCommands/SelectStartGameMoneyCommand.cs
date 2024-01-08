using App.Contracts.Requests;
using Mediator;

namespace App.SignalR.Commands.LobbyCommands;

public sealed record SelectStartGameMoneyCommand(
    SelectStartGameMoneyRequest Request) : ICommand<bool>;