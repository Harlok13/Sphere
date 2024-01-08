using App.Contracts.Requests;
using Mediator;

namespace App.SignalR.Commands.RoomCommands.PlayerActionCommands;

public sealed record KickPlayerFromRoomCommand(
    KickPlayerFromRoomRequest Request) : ICommand<bool>;