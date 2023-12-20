using App.Contracts.Responses;
using App.Domain.DomainEvents.RoomEvents;
using App.Domain.Entities.RoomEntity;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.SignalR.DomainEventHandlers.RoomEventHandlers;

public class CreateRoomEventHandler : INotificationHandler<CreateRoomEvent>
{
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly ILogger<CreateRoomEventHandler> _logger;

    public CreateRoomEventHandler(IHubContext<GlobalHub, IGlobalHub> hubContext, ILogger<CreateRoomEventHandler> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    
    public async ValueTask Handle(CreateRoomEvent notification, CancellationToken cT)
    {
        notification.Deconstruct(out Room room);

        var roomInLobbyResponse = new RoomInLobbyResponse(
            Id: room.Id,
            RoomName: room.RoomName,
            RoomSize: room.RoomSize,
            StartBid: room.StartBid,
            MaxBid: room.MaxBid,
            MinBid: room.MinBid,
            ImgUrl: room.AvatarUrl,
            Status: room.Status,
            PlayersInRoom: room.PlayersInRoom,
            Bank: room.Bank);
        await _hubContext.Clients.All.ReceiveAll_NewRoom(roomInLobbyResponse, cT);
        
        _logger.LogInformation($"{DateTime.UtcNow} | The new room has been sent to all users.");
    }
}