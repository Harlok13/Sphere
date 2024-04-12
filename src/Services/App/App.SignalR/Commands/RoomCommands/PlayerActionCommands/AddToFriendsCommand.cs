using App.Contracts.Requests;
using Mediator;

namespace App.SignalR.Commands.RoomCommands.PlayerActionCommands;

public sealed record AddToFriendsCommand(
    AddToFriendsRequest Request) : ICommand<bool>;