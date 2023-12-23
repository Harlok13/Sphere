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
            out Guid roomId, out Guid playerId, out int playersInRoom, out bool isLeader, out string connectionId);

        var response = new UpdatedPlayerIsLeaderResponse(playerId, isLeader);
        
        await _hubContext.Clients.Client(connectionId).ReceiveOwn_UpdatedPlayerIsLeader(response, cT);
        _logger.LogInformation(
            "{InvokedMethod} | The player \"{ConnectionId}\" received an updated \"{UpdatedValue}\" value.",
            nameof(_hubContext.Clients.All.ReceiveOwn_UpdatedPlayerIsLeader),
            nameof(playerId),
            connectionId);

        /*
         We don't need to check the number of players in the room, because this
         command changes the state of the room itself and does not depend on the
         number of players
         */
        await _hubContext.Clients.Group(roomId.ToString()).ReceiveGroup_UpdatedPlayerIsLeader(response, cT);
        _logger.LogInformation(
            "{InvokedMethod} | The group \"{RoomId}\" received an updated player \"{ConnectionId}\".",
            nameof(_hubContext.Clients.All.ReceiveGroup_UpdatedPlayerIsLeader),
            roomId,
            connectionId);
    }
}