using App.Contracts.Requests;
using Mediator;

namespace App.SignalR.Commands.RoomCommands.GameActionCommands;

public sealed record StayCommand(
    StayRequest Request) : ICommand<bool>;