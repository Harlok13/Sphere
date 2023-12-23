using App.Contracts.Responses.RoomResponses;
using App.Domain.DomainEvents.RoomDomainEvents;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.SignalR.DomainEventHandlers.RoomDomainEventHandlers;

public class ChangedRoomPlayersInRoomDomainEventHandler : INotificationHandler<ChangedRoomPlayersInRoomDomainEvent>
{
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly ILogger<ChangedRoomPlayersInRoomDomainEventHandler> _logger;

    public ChangedRoomPlayersInRoomDomainEventHandler(
        ILogger<ChangedRoomPlayersInRoomDomainEventHandler> logger,
        IHubContext<GlobalHub, IGlobalHub> hubContext)
    {
        _logger = logger;
        _hubContext = hubContext;
    }

    public async ValueTask Handle(ChangedRoomPlayersInRoomDomainEvent notification, CancellationToken cT)
    {
        notification.Deconstruct(out Guid roomId, out int playersInRoom);

        var response = new UpdatedRoomPlayersInRoomResponse(RoomId: roomId, PlayersInRoom: playersInRoom);

        await _hubContext.Clients.All.ReceiveAll_UpdatedRoomPlayersInRoom(response, cT);
        _logger.LogInformation(
            "{InvokedMethod} | The updated value \"{UpdatedValue}\" of room \"{RoomId}\" has been sent to all users.",
            nameof(_hubContext.Clients.All.ReceiveAll_UpdatedRoomPlayersInRoom),
            nameof(playersInRoom).ToUpper(),
            roomId);
    }
}