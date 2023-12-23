using App.Contracts.Mapper;
using App.Domain.DomainEvents.PlayerDomainEvents;
using App.Domain.Entities.PlayerEntity;
using App.Domain.Entities.RoomEntity;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.SignalR.DomainEventHandlers.PlayerDomainEventHandlers;

public class CreatedPlayerDomainEventHandler : INotificationHandler<CreatedPlayerDomainEvent>
{
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly ILogger<CreatedPlayerDomainEventHandler> _logger;

    public CreatedPlayerDomainEventHandler(
        IHubContext<GlobalHub, IGlobalHub> hubContext,
        ILogger<CreatedPlayerDomainEventHandler> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async ValueTask Handle(CreatedPlayerDomainEvent notification, CancellationToken cT)
    {
        notification.Deconstruct(out Player player, out Room room);

        await _hubContext.Groups.AddToGroupAsync(player.ConnectionId, room.Id.ToString(), cT);
        _logger.LogInformation(
            "{InvokedMethod} | The player \"{ConnectionId} - {PlayerName}\" was joined to the group \"{RoomId}\".",
            nameof(_hubContext.Groups.AddToGroupAsync),
            player.ConnectionId,
            player.PlayerName,
            room.Id);
        
        var playerResponse = PlayerMapper.MapPlayerToPlayerResponse(player);
        var initRoomResponse = RoomMapper.MapRoomToInitRoomDataResponse(room);
        var playersResponse = PlayerMapper.MapManyPlayersToManyPlayersResponse(room.Players);
        
        await _hubContext.Clients.Client(player.ConnectionId)
            .ReceiveOwn_PlayerData(playerResponse, initRoomResponse, playersResponse, cT);
        _logger.LogInformation(
            "{InvokedMethod} | The init data of room \"{RoomId}\" has been sent to player \"{ConnectionId}\".",
            nameof(_hubContext.Clients.All.ReceiveOwn_PlayerData),
            room.Id,
            player.ConnectionId);
    }
}