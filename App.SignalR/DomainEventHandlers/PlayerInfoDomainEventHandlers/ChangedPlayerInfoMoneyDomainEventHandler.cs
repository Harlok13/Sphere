using App.Contracts.Responses.PlayerInfoResponses;
using App.Domain.DomainEvents.PlayerInfoDomainEvents;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.SignalR.DomainEventHandlers.PlayerInfoDomainEventHandlers;

public class ChangedPlayerInfoMoneyDomainEventHandler : INotificationHandler<ChangedPlayerInfoMoneyDomainEvent>
{
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly ILogger<ChangedPlayerInfoMoneyDomainEventHandler> _logger;

    public ChangedPlayerInfoMoneyDomainEventHandler(
        IHubContext<GlobalHub, IGlobalHub> hubContext,
        ILogger<ChangedPlayerInfoMoneyDomainEventHandler> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async ValueTask Handle(ChangedPlayerInfoMoneyDomainEvent notification, CancellationToken cT)
    {
        notification.Deconstruct(out int money, out Guid playerId);

        var response = new ChangedPlayerInfoMoneyResponse(money);

        await _hubContext.Clients.User(playerId.ToString()).ReceiveOwn_ChangedPlayerInfoMoney(response, cT);
        _logger.LogInformation(
            "{InvokedMethod} - The changed value \"{ValueName} - {Value}\" has been sent to the player \"{PlayerId}\".",
            nameof(_hubContext.Clients.All.ReceiveOwn_ChangedPlayerInfoMoney),
            nameof(notification.Money),
            notification,
            playerId);
    }
}