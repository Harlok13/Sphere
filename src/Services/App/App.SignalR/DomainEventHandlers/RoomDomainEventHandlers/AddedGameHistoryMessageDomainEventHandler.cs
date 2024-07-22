using App.Contracts.Responses.RoomResponses;
using App.Domain.DomainEvents.RoomDomainEvents;
using App.Domain.Primitives;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.SignalR.DomainEventHandlers.RoomDomainEventHandlers;

public class AddedGameHistoryMessageDomainEventHandler : INotificationHandler<AddedGameHistoryMessageDomainEvent>
{
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly ILogger<AddedGameHistoryMessageDomainEventHandler> _logger;

    public AddedGameHistoryMessageDomainEventHandler(
        IHubContext<GlobalHub, IGlobalHub> hubContext,
        ILogger<AddedGameHistoryMessageDomainEventHandler> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }
    
    public async ValueTask Handle(AddedGameHistoryMessageDomainEvent notification, CancellationToken cT)
    {
        notification.Deconstruct(out Guid roomId, out GameHistoryMessage gameHistoryMessage);

        var response = new AddedGameHistoryMessageResponse(
            Type: gameHistoryMessage.Type.ToString(),
            CurrentTime: gameHistoryMessage.CurrentTime,
            Message: gameHistoryMessage.Message,
            PlayerName: gameHistoryMessage.PlayerName);
        
        await _hubContext.Clients.Group(roomId.ToString()).ReceiveGroup_AddedGameHistoryMessage(response, cT);
        _logger.LogInformation(
            "{InvokedMethod} - The new game history message \"{ValueName} - {Value}\" has been sent to the group \"{RoomId}\".",
            nameof(_hubContext.Clients.All.ReceiveGroup_ChangedRoomBank),
            nameof(notification.Message),
            gameHistoryMessage,
            roomId);
    }
}