using App.Contracts.Responses;
using App.SignalR.Events;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.SignalR.EventHandlers;

public class UserReconnectToRoomEventHandler : INotificationHandler<UserReconnectToRoomEvent>
{
    private readonly ILogger<UserReconnectToRoomEventHandler> _logger;
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;

    public UserReconnectToRoomEventHandler(
        IHubContext<GlobalHub, IGlobalHub> hubContext,
        ILogger<UserReconnectToRoomEventHandler> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }
    
    public async ValueTask Handle(UserReconnectToRoomEvent notification, CancellationToken cT)
    {
        notification.Deconstruct(out Guid roomId, out Guid playerId);

        var response = new ReconnectToRoomResponse(roomId);

        await _hubContext.Clients.User(playerId.ToString()).ReceiveUser_ReconnectToRoom(response, cT);
        _logger.LogInformation(
            "{InvokedMethod} - An offer to reconnect to the room \"{RoomId}\" has been sent to the player \"{PlayerId}\".",
            nameof(_hubContext.Clients.All.ReceiveUser_ReconnectToRoom),
            roomId,
            playerId);
    }
}