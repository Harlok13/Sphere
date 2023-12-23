using App.Contracts.Responses.RoomResponses;
using App.Domain.DomainEvents.RoomDomainEvents;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.SignalR.DomainEventHandlers.RoomDomainEventHandlers;

public class ChangedRoomBankDomainEventHandler : INotificationHandler<ChangedRoomBankDomainEvent>
{
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly ILogger<ChangedRoomBankDomainEventHandler> _logger;

    public ChangedRoomBankDomainEventHandler(
        ILogger<ChangedRoomBankDomainEventHandler> logger,
        IHubContext<GlobalHub, IGlobalHub> hubContext)
    {
        _logger = logger;
        _hubContext = hubContext;
    }
    
    public async ValueTask Handle(ChangedRoomBankDomainEvent notification, CancellationToken cT)
    {
        notification.Deconstruct(out Guid roomId, out int bank);

        var response = new ChangedRoomBankResponse(Bank: bank);

        await _hubContext.Clients.Group(roomId.ToString()).ReceiveGroup_ChangedRoomBank(response, cT);
        _logger.LogInformation(
            "{InvokedMethod} | The changed value \"Bank\" of room \"{RoomId}\" has been sent to the group.",
            nameof(_hubContext.Clients.All.ReceiveGroup_ChangedRoomBank),
            roomId);
    }
}