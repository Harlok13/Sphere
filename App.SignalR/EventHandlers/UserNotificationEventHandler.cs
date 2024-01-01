using App.Contracts.Responses;
using App.SignalR.Events;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.SignalR.EventHandlers;

public class UserNotificationEventHandler : INotificationHandler<UserNotificationEvent>
{
    private readonly ILogger<UserNotificationEventHandler> _logger;
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;

    public UserNotificationEventHandler(
        IHubContext<GlobalHub, IGlobalHub> hubContext,
        ILogger<UserNotificationEventHandler> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }
    
    public async ValueTask Handle(UserNotificationEvent notification, CancellationToken cT)
    {
        notification.Deconstruct(out string notificationText, out Guid targetId);
        
        var response = new NotificationResponse(Guid.NewGuid(), notificationText);
        
        await _hubContext.Clients.User(targetId.ToString()).ReceiveUser_Notification(response, cT);
        _logger.LogInformation(
            "Notification: {NotificationText} has been sent to \"{TargetId}\".",
            notificationText,
            targetId);
    }
}