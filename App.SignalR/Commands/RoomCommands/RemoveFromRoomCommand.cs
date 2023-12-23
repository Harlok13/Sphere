using App.Contracts.Requests;
using Mediator;

namespace App.SignalR.Commands.RoomCommands;

public sealed record RemoveFromRoomCommand(
    RemoveFromRoomRequest Request,
    string ConnectionId) : ICommand<bool>;