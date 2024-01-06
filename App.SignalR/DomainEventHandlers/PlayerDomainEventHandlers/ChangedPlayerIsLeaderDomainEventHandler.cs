using App.Contracts.Responses.PlayerResponses;
using App.Domain.DomainEvents.PlayerDomainEvents;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.SignalR.DomainEventHandlers.PlayerDomainEventHandlers;

public class ChangedPlayerIsLeaderDomainEventHandler : INotificationHandler<ChangedPlayerIsLeaderDomainEvent>
{
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly ILogger<ChangedPlayerIsLeaderDomainEventHandler> _logger;

    public ChangedPlayerIsLeaderDomainEventHandler(
        IHubContext<GlobalHub, IGlobalHub> hubContext,
        ILogger<ChangedPlayerIsLeaderDomainEventHandler> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async ValueTask Handle(ChangedPlayerIsLeaderDomainEvent notification, CancellationToken cT)
    {
        notification.Deconstruct(
            out Guid roomId, out Guid playerId, out bool isLeader, out string connectionId);

        var response = new ChangedPlayerIsLeaderResponse(playerId, isLeader);
        
        await _hubContext.Clients.Client(connectionId).ReceiveOwn_ChangedPlayerIsLeader(response, cT);
        _logger.LogInformation(
            "{InvokedMethod} - The changed value \"{ValueName} - {Value}\" has been sent to the player \"{ConnectionId}\".",
            nameof(_hubContext.Clients.All.ReceiveOwn_ChangedPlayerIsLeader),
            nameof(notification.IsLeader),
            isLeader,
            connectionId);

        /*
         We don't need to check the number of players in the room, because this
         command changes the state of the room itself and does not depend on the
         number of players
         */
        await _hubContext.Clients.Group(roomId.ToString()).ReceiveGroup_ChangedPlayerIsLeader(response, cT);
        _logger.LogInformation(
            "{InvokedMethod} - The changed value \"{ValueName} - {Value}\" has been sent to the group \"{RoomId}\".",
            nameof(_hubContext.Clients.All.ReceiveGroup_ChangedPlayerIsLeader),
            nameof(notification.IsLeader),
            isLeader,
            roomId);
    }
}