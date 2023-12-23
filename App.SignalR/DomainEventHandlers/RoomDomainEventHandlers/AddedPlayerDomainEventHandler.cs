using App.Contracts.Mapper;
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

        var playerResponse = PlayerMapper.MapPlayerToPlayerResponse(player);  // TODO: playerDto ?
        // TODO: response and change naming to AddedPlayer
        await _hubContext.Clients.GroupExcept(room.Id.ToString(), player.ConnectionId).ReceiveGroup_NewPlayer(playerResponse, cT);
        _logger.LogInformation(
            "{InvokedMethod} | The data of new player \"{ConnectionId}\" has been sent to the room \"{RoomId}\", except \"{ExceptedConnectionId}\".",
            nameof(_hubContext.Clients.All.ReceiveGroup_NewPlayer),
            player.ConnectionId,
            room.Id,
            player.ConnectionId);
    }
}