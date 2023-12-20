using App.SignalR.Commands.RoomCommands;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.RoomHandlers;

public class StartTimerHandler : ICommandHandler<StartTimerCommand, bool>
{
    private readonly ILogger<StartTimerHandler> _logger;
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly IDistributedCache _cache;
    private const int StartTimerValue = 1;
    private const int SecondsCount = 10;

    public StartTimerHandler(
        ILogger<StartTimerHandler> logger,
        IHubContext<GlobalHub, IGlobalHub> hubContext,
        IDistributedCache cache)
    {
        _logger = logger;
        _hubContext = hubContext;
        _cache = cache;
    }

    public async ValueTask<bool> Handle(StartTimerCommand command, CancellationToken cT)
    {
        command.Deconstruct(
            out Guid roomId, 
            out Guid playerId,
            out string connectionId, 
            out CancellationTokenSource cts);
        
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

            return false;
        }, cT);
        

        // var timeOutCommand = new TimeOutCommand(roomId, userId);
        // await _mediator.Send(timeOutCommand, cT);
        await Task.Delay(1);
        return false;
    }
}

