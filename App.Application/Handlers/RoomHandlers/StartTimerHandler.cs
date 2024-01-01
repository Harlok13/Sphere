using App.Contracts.Requests;
using App.SignalR.Commands.RoomCommands;
using App.SignalR.Commands.RoomCommands.GameActionCommands;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.RoomHandlers;

public class StartTimerHandler : ICommandHandler<StartTimerCommand, bool>
{
    private readonly ILogger<StartTimerHandler> _logger;
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly IMediator _mediator;
    private const int StartTimerValue = 1;
    private const int SecondsCount = 10;

    public StartTimerHandler(
        ILogger<StartTimerHandler> logger,
        IHubContext<GlobalHub, IGlobalHub> hubContext,
        IMediator mediator)
    {
        _logger = logger;
        _hubContext = hubContext;
        _mediator = mediator;
    }

    public async ValueTask<bool> Handle(StartTimerCommand command, CancellationToken cT)
    {
        command.Request.Deconstruct(
            out Guid roomId, 
            out Guid playerId);
        var connectionId = command.ConnectionId;
        var cts = command.Cts;

        _ = Task.Run(async () =>
        {
            foreach (var i in Enumerable.Range(StartTimerValue, SecondsCount))
            {
                if (cts.Token.IsCancellationRequested)
                {
                    _logger.LogInformation("StartTimer cancelled.");
                    return true;
                }

                var seconds = i;
                await _hubContext.Clients.Client(connectionId).ReceiveOwn_UpdatedTimer(seconds, cT);
                await Task.Delay(1000, cT);
                
                _logger.LogInformation($"Time has passed: {seconds}");
            }
            await _hubContext.Clients.Client(connectionId).ReceiveClient_TimeOut(cT);
            _logger.LogInformation("en!! StartTimer end of work.");

            return false;
        }, cT);

        await Task.Delay(10, cT);
        return false;
    }
}

