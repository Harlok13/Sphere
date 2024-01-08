using App.Contracts.Responses.PlayerResponses;
using App.Domain.DomainEvents.PlayerDomainEvents;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.SignalR.DomainEventHandlers.PlayerDomainEventHandlers;

public class ChangedPlayerReadinessDomainEventHandler : INotificationHandler<ChangedPlayerReadinessDomainEvent>
{
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly ILogger<ChangedPlayerReadinessDomainEventHandler> _logger;

    public ChangedPlayerReadinessDomainEventHandler(
        IHubContext<GlobalHub, IGlobalHub> hubContext,
        ILogger<ChangedPlayerReadinessDomainEventHandler> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async ValueTask Handle(ChangedPlayerReadinessDomainEvent notification, CancellationToken cT)
    {
        notification.Deconstruct(out bool readiness, out string connectionId, out Guid roomId, out Guid playerId);

        var response = new ChangedPlayerReadinessResponse(PlayerId: playerId, Readiness: readiness);

        await _hubContext.Clients.User(playerId.ToString()).ReceiveUser_ChangedPlayerReadiness(response, cT);
        _logger.LogInformation(
            "{InvokedMethod} - The changed value \"{ValueName} - {Value}\" has been sent to the player \"{PlayerId}\".",
            nameof(_hubContext.Clients.All.ReceiveUser_ChangedPlayerReadiness),
            nameof(notification.Readiness),
            readiness,
            playerId);

        await _hubContext.Clients.Group(roomId.ToString()).ReceiveGroup_ChangedPlayerReadiness(response, cT);
        _logger.LogInformation(
            "{InvokedMethod} - The changed value \"{ValueName} - {Value}\" has been sent to the group \"{RoomId}\".",
            nameof(_hubContext.Clients.All.ReceiveGroup_ChangedPlayerReadiness),
            nameof(notification.Readiness),
            readiness,
            roomId);
    }
}