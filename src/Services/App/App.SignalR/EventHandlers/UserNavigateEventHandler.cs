using App.Contracts.Enums;
using App.Contracts.Responses;
using App.SignalR.Events;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.SignalR.EventHandlers;

public class UserNavigateEventHandler : INotificationHandler<UserNavigateEvent>
{
    private readonly ILogger<UserNavigateEventHandler> _logger;
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;

    public UserNavigateEventHandler(
        IHubContext<GlobalHub, IGlobalHub> hubContext,
        ILogger<UserNavigateEventHandler> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }
    
    public async ValueTask Handle(UserNavigateEvent notification, CancellationToken cT)
    {
        notification.Deconstruct(out Guid targetId, out NavigateEnum navigate);

        var response = new NavigateResponse(navigate.ToString());

        await _hubContext.Clients.User(targetId.ToString()).ReceiveUser_Navigate(response, cT);
        _logger.LogInformation(
            "{InvokedMethod} - The navigate value \"{Value}\" has been sent to the player \"{PlayerId}\".",
            nameof(_hubContext.Clients.All.ReceiveUser_Navigate),
            navigate.ToString(),
            targetId);
    }
}