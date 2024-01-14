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

        var response = new ChangedRoomStatusResponse(RoomId: roomId, Status: roomStatus.ToString());
        
        await _hubContext.Clients.All.ReceiveAll_ChangedRoomStatus(response, cT);
        _logger.LogInformation(
            "{InvokedMethod} - The changed value \"{ValueName} - {Value}\" of room \"{RoomId}\" has been sent to all users.",
            nameof(_hubContext.Clients.All.ReceiveAll_ChangedRoomStatus),
            nameof(notification.RoomStatus),
            roomStatus,
            roomId);
        
        await _hubContext.Clients.All.ReceiveGroup_ChangedRoomStatus(response, cT);
        _logger.LogInformation(
            "{InvokedMethod} - The changed value \"{ValueName} - {Value}\" of room \"{RoomId}\" has been sent to group.",
            nameof(_hubContext.Clients.All.ReceiveAll_ChangedRoomStatus),
            nameof(notification.RoomStatus),
            roomStatus,
            roomId);
    }
}