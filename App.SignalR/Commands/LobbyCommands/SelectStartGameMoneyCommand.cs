using Mediator;

namespace App.SignalR.Commands.LobbyCommands;

public sealed record SelectStartGameMoneyCommand(
    Guid RoomId,
    Guid PlayerId,
    int StartBid,
    int MinBid,
    int MaxBid,
    string ConnectionId) : ICommand<bool>;