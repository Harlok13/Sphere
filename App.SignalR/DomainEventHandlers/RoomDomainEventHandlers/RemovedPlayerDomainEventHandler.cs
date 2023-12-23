using App.Contracts.Responses.RoomResponses;
using App.Domain.DomainEvents.RoomDomainEvents;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.SignalR.DomainEventHandlers.RoomDomainEventHandlers;

public class RemovedPlayerDomainEventHandler : INotificationHandler<RemovedPlayerDomainEvent>
{
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly ILogger<RemovedPlayerDomainEventHandler> _logger;

    public RemovedPlayerDomainEventHandler(
        IHubContext<GlobalHub, IGlobalHub> hubContext,
        ILogger<RemovedPlayerDomainEventHandler> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }
    
    public async ValueTask Handle(RemovedPlayerDomainEvent notification, CancellationToken cT)
    {
        notification.Deconstruct(out Guid roomId, out Guid playerId, out string connectionId, out int playersInRoom);

        /* Reset the player state */
        await _hubContext.Clients.Client(connectionId).ReceiveOwn_RemoveFromRoom(cT);
        _logger.LogInformation(
            "{InvokedMethod} | A command to leave the room \"{RoomId}\" has been sent to player \"{ConnectionId}\".",
            nameof(_hubContext.Clients.All.ReceiveOwn_RemoveFromRoom),
            roomId,
            connectionId);

        await _hubContext.Groups.RemoveFromGroupAsync(connectionId, roomId.ToString(), cT);
        _logger.LogInformation(
            "{InvokedMethod} | The player \"{ConnectionId}\" has been removed from group \"{RoomId}\".",
            nameof(_hubContext.Groups.RemoveFromGroupAsync),
            connectionId,
            roomId);
            
        if (playersInRoom > 0)
        {
            var response = new RemovedPlayerResponse(playerId);

            await _hubContext.Clients.Group(roomId.ToString()).ReceiveGroup_RemovedPlayer(response, cT);
            _logger.LogInformation(
                "{InvokedMethod} | The removed player \"{ConnectionId}\" from room \"{RoomId}\" has been sent to group.",
                nameof(_hubContext.Clients.All.ReceiveGroup_RemovedPlayer),
                connectionId,
                roomId);
        }
    }
}
