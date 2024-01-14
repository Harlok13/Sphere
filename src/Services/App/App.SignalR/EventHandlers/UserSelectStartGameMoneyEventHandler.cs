using App.Contracts.Responses;
using App.SignalR.Events;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.SignalR.EventHandlers;

public class UserSelectStartGameMoneyEventHandler : INotificationHandler<UserSelectStartGameMoneyEvent>
{
    private readonly ILogger<UserSelectStartGameMoneyEventHandler> _logger;
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;

    public UserSelectStartGameMoneyEventHandler(
        IHubContext<GlobalHub, IGlobalHub> hubContext,
        ILogger<UserSelectStartGameMoneyEventHandler> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async ValueTask Handle(UserSelectStartGameMoneyEvent notification, CancellationToken cT)
    {
        notification.Deconstruct(out Guid playerId, out SelectStartGameMoneyResponse response);

        await _hubContext.Clients.User(playerId.ToString()).ReceiveUser_SelectStartGameMoney(response, cT);
        _logger.LogInformation(
            "{InvokedMethod} - An offer to select starting game money has been sent to the player \"{PlayerId}\".",
            nameof(_hubContext.Clients.All.ReceiveUser_ReconnectToRoom),
            playerId);
    }
}