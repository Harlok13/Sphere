using App.Contracts.Responses.RoomResponses;
using App.Domain.DomainEvents.RoomDomainEvents;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.SignalR.DomainEventHandlers.RoomDomainEventHandlers;

public class ChangedRoomRoomNameDomainEventHandler : INotificationHandler<ChangedRoomRoomNameDomainEvent>
{
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly ILogger<ChangedRoomRoomNameDomainEventHandler> _logger;

    public ChangedRoomRoomNameDomainEventHandler(
        ILogger<ChangedRoomRoomNameDomainEventHandler> logger,
        IHubContext<GlobalHub, IGlobalHub> hubContext)
    {
        _logger = logger;
        _hubContext = hubContext;
    }
    
    public async ValueTask Handle(ChangedRoomRoomNameDomainEvent notification, CancellationToken cT)
    {
        notification.Deconstruct(out Guid roomId, out string roomName);

        var response = new ChangedRoomRoomNameResponse(RoomId: roomId, RoomName: roomName);

        await _hubContext.Clients.All.ReceiveAll_ChangedRoomRoomName(response, cT);
        _logger.LogInformation(
            "{InvokedMethod} | The updated value \"{UpdatedValue}\" of room \"{RoomId}\" has been sent to all users.",
            nameof(_hubContext.Clients.All.ReceiveAll_ChangedRoomRoomName),
            nameof(roomName).ToUpper(),
            roomId);

        await _hubContext.Clients.Group(roomId.ToString()).ReceiveGroup_ChangedRoomRoomName(response, cT);
        _logger.LogInformation(
            "{InvokedMethod} | The updated value \"{UpdatedValue}\" of room \"{RoomId}\" has been sent to the group.",
            nameof(_hubContext.Clients.All.ReceiveGroup_ChangedRoomRoomName),
            nameof(roomName).ToUpper(),
            roomId);
    }
}