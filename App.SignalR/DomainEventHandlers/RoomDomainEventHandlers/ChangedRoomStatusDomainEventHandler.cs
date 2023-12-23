using App.Contracts.Responses.RoomResponses;
using App.Domain.DomainEvents.RoomDomainEvents;
using App.Domain.Enums;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.SignalR.DomainEventHandlers.RoomDomainEventHandlers;

public class ChangedRoomStatusDomainEventHandler : INotificationHandler<ChangedRoomStatusDomainEvent>
{
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly ILogger<ChangedRoomStatusDomainEventHandler> _logger;

    public ChangedRoomStatusDomainEventHandler(
        IHubContext<GlobalHub, IGlobalHub> hubContext, 
        ILogger<ChangedRoomStatusDomainEventHandler> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async ValueTask Handle(ChangedRoomStatusDomainEvent notification, CancellationToken cT)
    {
        notification.Deconstruct(out Guid roomId, out RoomStatus roomStatus);

        var response = new UpdatedRoomStatusResponse(RoomId: roomId, RoomStatus: roomStatus);
        
        await _hubContext.Clients.All.ReceiveAll_UpdatedRoomStatus(response, cT);
        _logger.LogInformation(
            "{InvokedMethod} | The updated value \"{UpdatedValue}\" of room \"{RoomId}\" has been sent to all users.",
            nameof(_hubContext.Clients.All.ReceiveAll_UpdatedRoomStatus),
            nameof(roomStatus).ToUpper(),
            roomId);
    }
}