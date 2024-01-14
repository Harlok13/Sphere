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
        notification.Deconstruct(out int money, out Guid roomId, out Guid playerId);

        var response = new ChangedPlayerMoneyResponse(PlayerId: playerId, Money: money);

        await _hubContext.Clients.User(playerId.ToString()).ReceiveUser_ChangedPlayerMoney(response, cT);
        _logger.LogInformation(
            "{InvokedMethod} - The changed value \"{ValueName} - {Value}\" has been sent to the player \"{PlayerId}\".",
            nameof(_hubContext.Clients.All.ReceiveUser_ChangedPlayerMoney),
            nameof(notification.Money),
            money,
            playerId);

        await _hubContext.Clients.Group(roomId.ToString()).ReceiveGroup_ChangedPlayerMoney(response, cT);
        _logger.LogInformation(
            "{InvokedMethod} - The changed value \"{ValueName} - {Value}\" of player \"{PlayerId}\" has been sent to the group \"{RoomId}\".",
            nameof(_hubContext.Clients.All.ReceiveGroup_ChangedPlayerMoney),
            nameof(notification.Money),
            money,
            playerId,
            roomId);
    }
}

