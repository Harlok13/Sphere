using System.Diagnostics;
using App.Contracts.Mapper;
using App.Contracts.Responses.PlayerResponses;
using App.Domain.DomainEvents.PlayerDomainEvents;
using App.Domain.Entities;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.SignalR.DomainEventHandlers.PlayerDomainEventHandlers;

public class AddedCardDomainEventHandler : INotificationHandler<AddedCardDomainEvent>
{
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly ILogger<AddedCardDomainEventHandler> _logger;

    public AddedCardDomainEventHandler(
        ILogger<AddedCardDomainEventHandler> logger, 
        IHubContext<GlobalHub, IGlobalHub> hubContext)
    {
        _logger = logger;
        _hubContext = hubContext;
    }
    
    public async ValueTask Handle(AddedCardDomainEvent notification, CancellationToken cT)
    {
        var sw = new Stopwatch();
        
        sw.Start();
        notification.Deconstruct(
            out Card card, out int delayMs, out Guid roomId, out Guid playerId);

        var cardDto = CardMapper.MapCardToCardDto(card);
        var response = new AddedCardResponse(PlayerId: playerId, CardDto: cardDto);
        sw.Stop();

        var resultDelay = delayMs - sw.Elapsed.Milliseconds < 0
            ? 0
            : delayMs - sw.Elapsed.Milliseconds;
        
        _logger.LogDebug(
            "Stopwatch time elapsed: {Elapsed} ms. The resulting delay: {Delay} ms",
            sw.Elapsed.Milliseconds,
            resultDelay);

        await Task.Delay(resultDelay, cT);

        await _hubContext.Clients.User(playerId.ToString()).ReceiveUser_AddedCard(response, cT);
        _logger.LogInformation(
            "{InvokedMethod} - The new card \"{ValueName} - {Value}\" has been sent to the player \"{PlayerId}\" with delay {Delay} ms.",
            nameof(_hubContext.Clients.All.ReceiveUser_AddedCard),
            nameof(notification.Card),
            card.SuitValue,
            playerId,
            resultDelay);

        await _hubContext.Clients.Group(roomId.ToString()).ReceiveGroup_AddedCard(response, cT);
        _logger.LogInformation(
            "{InvokedMethod} - The new card \"{ValueName} - {Value}\" has been sent to the group \"{RoomId}\" with delay {Delay} ms.",
            nameof(_hubContext.Clients.All.ReceiveGroup_AddedCard),
            nameof(notification.Card),
            card,
            roomId,
            resultDelay);
    }
}

