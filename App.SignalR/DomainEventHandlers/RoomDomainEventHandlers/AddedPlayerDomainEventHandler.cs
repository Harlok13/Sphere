using App.Contracts.Mapper;
using App.Contracts.Responses.RoomResponses;
using App.Domain.DomainEvents.RoomDomainEvents;
using App.Domain.Entities.PlayerEntity;
using App.Domain.Entities.RoomEntity;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.SignalR.DomainEventHandlers.RoomDomainEventHandlers;

public class AddedPlayerDomainEventHandler : INotificationHandler<AddedPlayerDomainEvent>
{
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly ILogger<AddedPlayerDomainEventHandler> _logger;

    public AddedPlayerDomainEventHandler(
        IHubContext<GlobalHub, IGlobalHub> hubContext,
        ILogger<AddedPlayerDomainEventHandler> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async ValueTask Handle(AddedPlayerDomainEvent notification, CancellationToken cT)
    {
        notification.Deconstruct(out Player player, out Room room);

        var playerResponse = PlayerMapper.MapPlayerToPlayerDto(player); 
        var response = new AddedPlayerResponse(playerResponse);
        
        await _hubContext.Clients.GroupExcept(room.Id.ToString(), player.ConnectionId).ReceiveGroup_AddedPlayer(response, cT);
        _logger.LogInformation(
            "{InvokedMethod} | The data of new player \"{ConnectionId}\" has been sent to the room \"{RoomId}\", except \"{ExceptedConnectionId}\".",
            nameof(_hubContext.Clients.All.ReceiveGroup_AddedPlayer),
            player.ConnectionId,
            room.Id,
            player.ConnectionId);
    }
}