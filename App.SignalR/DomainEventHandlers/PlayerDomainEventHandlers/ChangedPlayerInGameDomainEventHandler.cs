using App.Contracts.Responses.PlayerResponses;
using App.Domain.DomainEvents.PlayerDomainEvents;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.SignalR.DomainEventHandlers.PlayerDomainEventHandlers;

public class ChangedPlayerInGameDomainEventHandler : INotificationHandler<ChangedPlayerInGameDomainEvent>
{
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly ILogger<ChangedPlayerInGameDomainEventHandler> _logger;

    public ChangedPlayerInGameDomainEventHandler(
        IHubContext<GlobalHub, IGlobalHub> hubContext,
        ILogger<ChangedPlayerInGameDomainEventHandler> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async ValueTask Handle(ChangedPlayerInGameDomainEvent notification, CancellationToken cT)
    {
        notification.Deconstruct(out Guid roomId, out Guid playerId, out bool inGame, out string connectionId);

        var response = new ChangedPlayerInGameResponse(PlayerId: playerId, InGame: inGame);

        await _hubContext.Clients.Client(connectionId).ReceiveOwn_ChangedPlayerInGame(response, cT);
        _logger.LogInformation(
            "{InvokedMethod} - The changed value \"{ValueName} - {Value}\" has been sent to the player \"{ConnectionId}\".",
            nameof(_hubContext.Clients.All.ReceiveOwn_ChangedPlayerInGame),
            nameof(notification.InGame),
            inGame,
            connectionId);

        await _hubContext.Clients.Group(roomId.ToString()).ReceiveGroup_ChangedPlayerInGame(response, cT);
        _logger.LogInformation(
            "{InvokedMethod} - The changed value \"{ValueName} - {Value}\" has been sent to the group \"{RoomId}\".",
            nameof(_hubContext.Clients.All.ReceiveGroup_ChangedPlayerInGame),
            nameof(notification.InGame),
            inGame,
            roomId);
    }
}