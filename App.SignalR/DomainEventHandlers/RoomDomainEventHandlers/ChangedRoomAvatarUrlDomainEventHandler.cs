using App.Contracts.Responses.RoomResponses;
using App.Domain.DomainEvents.RoomDomainEvents;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.SignalR.DomainEventHandlers.RoomDomainEventHandlers;

public class ChangedRoomAvatarUrlDomainEventHandler : INotificationHandler<ChangedRoomAvatarUrlDomainEvent>
{
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly ILogger<ChangedRoomAvatarUrlDomainEventHandler> _logger;

    public ChangedRoomAvatarUrlDomainEventHandler(
        ILogger<ChangedRoomAvatarUrlDomainEventHandler> logger,
        IHubContext<GlobalHub, IGlobalHub> hubContext)
    {
        _logger = logger;
        _hubContext = hubContext;
    }
    
    public async ValueTask Handle(ChangedRoomAvatarUrlDomainEvent notification, CancellationToken cT)
    {
        notification.Deconstruct(out Guid roomId, out string avatarUrl);

        var response = new ChangedRoomAvatarUrlResponse(RoomId: roomId, AvatarUrl: avatarUrl);

        await _hubContext.Clients.All.ReceiveAll_ChangedRoomAvatarUrl(response, cT);
        _logger.LogInformation(
            "{InvokedMethod} | The updated value \"{UpdatedValue}\" of room \"{RoomId}\" has been sent to all users.",
            nameof(_hubContext.Clients.All.ReceiveAll_ChangedRoomAvatarUrl),
            nameof(avatarUrl).ToUpper(),
            roomId);
    }
}