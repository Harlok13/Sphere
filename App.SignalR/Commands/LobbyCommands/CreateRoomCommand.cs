using App.Contracts.Requests;
using Mediator;

namespace App.SignalR.Commands.LobbyCommands;

public sealed record CreateRoomCommand(
    RoomRequest RoomRequest,
    Guid PlayerId,
    string PlayerName,
    string ConnectionId,
    bool IsLeader = true
) : ICommand<bool>;