using App.Contracts.Data;
using App.Contracts.Mapper;
using App.Contracts.Responses.PlayerResponses;
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
            "{InvokedMethod} - The player \"{ConnectionId} - {PlayerName}\" was joined to the group \"{RoomId}\".",
            nameof(_hubContext.Groups.AddToGroupAsync),
            player.ConnectionId,
            player.PlayerName,
            room.Id);
        
        var playerDto = PlayerMapper.MapPlayerToPlayerDto(player);
        var initRoomDataDto = RoomMapper.MapRoomToInitRoomDataDto(room);
        var playersDto = PlayerMapper.MapManyPlayersToManyPlayersDto(room.Players);

        var response = new CreatedPlayerResponse(
            Player: playerDto,
            InitRoomData: initRoomDataDto,
            Players: playersDto);
        
        await _hubContext.Clients.User(player.Id.ToString()).ReceiveOwn_CreatedPlayer(response, cT);
        _logger.LogInformation(
            "{InvokedMethod} - The init data of room \"{RoomId} - {RoomName}\" has been sent to the player \"{ConnectionId}\".",
            nameof(_hubContext.Clients.All.ReceiveOwn_CreatedPlayer),
            room.Id,
            room.RoomName,
            player.ConnectionId);
    }
}