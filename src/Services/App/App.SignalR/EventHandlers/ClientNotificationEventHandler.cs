using App.Contracts.Responses;
using App.SignalR.Events;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.SignalR.EventHandlers;

public class ClientNotificationEventHandler : INotificationHandler<ClientNotificationEvent>
{
    private readonly ILogger<ClientNotificationEventHandler> _logger;
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;

    public ClientNotificationEventHandler(
        IHubContext<GlobalHub, IGlobalHub> hubContext,
        ILogger<ClientNotificationEventHandler> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }


    public async ValueTask Handle(ClientNotificationEvent notification, CancellationToken cT)
    {
        notification.Deconstruct(out string notificationText, out string targetConnectionId);
        
        var response = new NotificationResponse(Guid.NewGuid(), notificationText);
        
        await _hubContext.Clients.Client(targetConnectionId).ReceiveClient_Notification(response, cT);
        _logger.LogInformation(
            "Notification: {NotificationText} has been sent to \"{ConnectionId}\".",
            notificationText,
            targetConnectionId);
    }
}