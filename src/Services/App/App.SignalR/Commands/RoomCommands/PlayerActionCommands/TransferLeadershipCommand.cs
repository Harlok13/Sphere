using App.Contracts.Requests;
using Mediator;

namespace App.SignalR.Commands.RoomCommands.PlayerActionCommands;

public sealed record TransferLeadershipCommand(
    TransferLeadershipRequest Request) : ICommand<bool>;