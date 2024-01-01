using App.Contracts.Responses.PlayerResponses;
using App.Domain.DomainEvents.PlayerDomainEvents;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.SignalR.DomainEventHandlers.PlayerDomainEventHandlers;

public class ChangedPlayerOnlineDomainEventHandler : INotificationHandler<ChangedPlayerOnlineDomainEvent>
{
    private readonly ILogger<ChangedPlayerOnlineDomainEventHandler> _logger;
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;

    public ChangedPlayerOnlineDomainEventHandler(
        ILogger<ChangedPlayerOnlineDomainEventHandler> logger,
        IHubContext<GlobalHub, IGlobalHub> hubContext)
    {
        _logger = logger;
        _hubContext = hubContext;
    }
    
    public async ValueTask Handle(ChangedPlayerOnlineDomainEvent notification, CancellationToken cT)
    {
        notification.Deconstruct(out bool online, out Guid roomId, out Guid playerId, out string connectionId);
        
        await _hubContext.Groups.AddToGroupAsync(connectionId, roomId.ToString(), cT);
        
        var response = new ChangedPlayerOnlineResponse(Online: online, PlayerId: playerId);

        await _hubContext.Clients.Client(connectionId).ReceiveClient_ChangedPlayerOnline(response, cT);
        
        await _hubContext.Clients.GroupExcept(roomId.ToString(), connectionId).ReceiveGroup_ChangedPlayerOnline(response, cT);
        _logger.LogInformation("");
        
        // TODO: send notification about connect/disconnect 
    }
}