using App.Contracts.Requests;
using App.SignalR.Commands.RoomCommands.PlayerActionCommands;

namespace App.SignalR.Hubs;

public partial class GlobalHub
{
    public async ValueTask<bool> AddToFriends(AddToFriendsRequest request) =>
        await _mediator.Send(new AddToFriendsCommand(request));
}