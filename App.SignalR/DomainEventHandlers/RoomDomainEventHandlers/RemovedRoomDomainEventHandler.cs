using App.Contracts.Responses.RoomResponses;
using App.Domain.DomainEvents.RoomDomainEvents;
using App.SignalR.Commands.RoomCommands;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.SignalR.DomainEventHandlers.RoomDomainEventHandlers;

public class RemovedRoomDomainEventHandler : INotificationHandler<RemovedRoomDomainEvent>
{
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly ILogger<RemovedRoomDomainEventHandler> _logger;
    private readonly IMediator _mediator; 

    public RemovedRoomDomainEventHandler(
        IHubContext<GlobalHub, IGlobalHub> hubContext,
        ILogger<RemovedRoomDomainEventHandler> logger, 
        IMediator mediator)
    {
        _hubContext = hubContext;
        _logger = logger;
        _mediator = mediator;
    }
    
    public async ValueTask Handle(RemovedRoomDomainEvent notification, CancellationToken cT)
    {
        notification.Deconstruct(out Guid roomId);

        var removeResult = await _mediator.Send(new RemoveRoomCommand(roomId), cT);
        if (!removeResult)
        {
            _logger.LogError("Deleting a room with ID \"{RoomId}\" failed.", roomId);
            return;
        }

        var response = new RemovedRoomResponse(roomId);
        
        await _hubContext.Clients.All.ReceiveAll_RemovedRoom(response, cT);
        _logger.LogInformation(
            "The remove room \"{RoomId}\" has been sent to all users.",
            roomId);
    }
}