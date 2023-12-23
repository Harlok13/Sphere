using App.Contracts.Mapper;
using App.Contracts.Responses.RoomResponses;
using App.Domain.DomainEvents.RoomDomainEvents;
using App.Domain.Entities.RoomEntity;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.SignalR.DomainEventHandlers.RoomDomainEventHandlers;

public class CreatedRoomDomainEventHandler : INotificationHandler<CreatedRoomDomainEvent>
{
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly ILogger<CreatedRoomDomainEventHandler> _logger;

    public CreatedRoomDomainEventHandler(
        IHubContext<GlobalHub, IGlobalHub> hubContext, 
        ILogger<CreatedRoomDomainEventHandler> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }
    
    public async ValueTask Handle(CreatedRoomDomainEvent notification, CancellationToken cT)
    {
        notification.Deconstruct(out Room room);

        var roomInLobbyDto = RoomMapper.MapRoomToRoomInLobbyDto(room);
        var response = new CreatedRoomResponse(roomInLobbyDto);
        
        await _hubContext.Clients.All.ReceiveAll_CreatedRoom(response, cT);
        _logger.LogInformation(
            "{InvokedMethod} | The new room \"{RoomId} - {Value}\" has been sent to all users.",
            nameof(_hubContext.Clients.All.ReceiveAll_CreatedRoom),
            room.Id,
            room);
    }
}