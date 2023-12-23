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
        notification.Deconstruct(out int money, out string connectionId);

        await _hubContext.Clients.Client(connectionId).ReceiveOwn_UpdatedMoney(money, cT);
        _logger.LogInformation(
            "{InvokedMethod} | A new value for property \"{UpdatedProperty}\" has been sent to player \"{ConnectionId}\".",
            nameof(_hubContext.Clients.All.ReceiveOwn_UpdatedMoney),
            nameof(notification.Money),
            connectionId);
    }
}