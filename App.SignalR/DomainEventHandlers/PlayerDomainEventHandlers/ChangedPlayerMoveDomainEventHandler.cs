using App.Contracts.Responses.PlayerResponses;
using App.Domain.DomainEvents.PlayerDomainEvents;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.SignalR.DomainEventHandlers.PlayerDomainEventHandlers;

public class ChangedPlayerMoveDomainEventHandler : INotificationHandler<ChangedPlayerMoveDomainEvent>
{
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly ILogger<ChangedPlayerMoveDomainEventHandler> _logger;

    public ChangedPlayerMoveDomainEventHandler(
        IHubContext<GlobalHub, IGlobalHub> hubContext,
        ILogger<ChangedPlayerMoveDomainEventHandler> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async ValueTask Handle(ChangedPlayerMoveDomainEvent notification, CancellationToken cT)
    {
        notification.Deconstruct(out bool move, out Guid playerId);

        var response = new ChangedPlayerMoveResponse(Move: move);

        await _hubContext.Clients.User(playerId.ToString()).ReceiveUser_ChangedPlayerMove(response, cT);
        _logger.LogInformation(
            "{InvokedMethod} - The changed value \"{ValueName} - {Value}\" has been sent to the player \"{PlayerId}\".",
            nameof(_hubContext.Clients.All.ReceiveUser_ChangedPlayerMove),
            nameof(notification.Move),
            move,
            playerId);
        
        // TODO: send to group (need to see whose turn it is now and the timer itself)
    }
}