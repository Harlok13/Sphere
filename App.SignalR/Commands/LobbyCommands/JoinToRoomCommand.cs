using Mediator;

namespace App.SignalR.Commands.LobbyCommands;

public sealed record JoinToRoomCommand(
    Guid RoomId,
    Guid PlayerId,
    // string PlayerName, 
    int SelectedStartMoney,
    string ConnectionId) : ICommand<bool>;