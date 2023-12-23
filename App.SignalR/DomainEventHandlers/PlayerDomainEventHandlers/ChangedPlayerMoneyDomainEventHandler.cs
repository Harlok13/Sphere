using App.Contracts.Responses.PlayerResponses;
using App.Domain.DomainEvents.PlayerDomainEvents;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.SignalR.DomainEventHandlers.PlayerDomainEventHandlers;

public class ChangedPlayerMoneyDomainEventHandler : INotificationHandler<ChangedPlayerMoneyDomainEvent>
{
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly ILogger<ChangedPlayerMoneyDomainEventHandler> _logger;

    public ChangedPlayerMoneyDomainEventHandler(
        ILogger<ChangedPlayerMoneyDomainEventHandler> logger, 
        IHubContext<GlobalHub, IGlobalHub> hubContext)
    {
        _logger = logger;
        _hubContext = hubContext;
    }

    public async ValueTask Handle(ChangedPlayerMoneyDomainEvent notification, CancellationToken cT)
    {
        notification.Deconstruct(out int money, out string connectionId, out Guid roomId, out Guid playerId);

        var response = new ChangedPlayerMoneyResponse(PlayerId: playerId, Money: money);

        await _hubContext.Clients.Client(connectionId).ReceiveOwn_ChangedPlayerMoney(response, cT);
        _logger.LogInformation(
            "{InvokedMethod} | The changed value \"Money\" has been sent to the player {ConnectionId}.",
            nameof(_hubContext.Clients.All.ReceiveOwn_ChangedPlayerMoney),
            connectionId);

        await _hubContext.Clients.Group(roomId.ToString()).ReceiveGroup_ChangedPlayerMoney(response, cT);
        _logger.LogInformation(
            "{InvokedMethod} | The changed value \"Money\" of player \"{ConnectionId}\" has been sent to the group {RoomId}.",
            nameof(_hubContext.Clients.All.ReceiveGroup_ChangedPlayerMoney),
            connectionId,
            roomId);
    }
}

