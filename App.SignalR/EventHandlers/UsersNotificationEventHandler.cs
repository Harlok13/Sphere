using App.Contracts.Responses;
using App.SignalR.Events;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.SignalR.EventHandlers;

public class UsersNotificationEventHandler : INotificationHandler<UsersNotificationEvent>
{
    private readonly ILogger<UsersNotificationEventHandler> _logger;
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;

    public UsersNotificationEventHandler(
        IHubContext<GlobalHub, IGlobalHub> hubContext,
        ILogger<UsersNotificationEventHandler> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }
    
    public async ValueTask Handle(UsersNotificationEvent notification, CancellationToken cT)
    {
        notification.Deconstruct(out string notificationText, out IEnumerable<Guid> playerIds);

        var response = new NotificationResponse(Guid.NewGuid(), notificationText);

        await _hubContext.Clients.Users(playerIds.Select(x => x.ToString())).ReceiveUser_Notification(response, cT);
        _logger.LogInformation(
            "Notification: {NotificationText} has been sent to users.",
            notificationText);
    }
}