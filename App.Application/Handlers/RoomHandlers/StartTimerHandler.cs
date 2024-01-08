using App.SignalR.Commands.RoomCommands;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.RoomHandlers;

public class StartTimerHandler : ICommandHandler<StartTimerCommand, bool>
{
    private readonly ILogger<StartTimerHandler> _logger;
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private const int StartTimerValue = 1;
    private const int SecondsCount = 10;

    public StartTimerHandler(
        ILogger<StartTimerHandler> logger,
        IHubContext<GlobalHub, IGlobalHub> hubContext)
    {
        _logger = logger;
        _hubContext = hubContext;
    }

    public async ValueTask<bool> Handle(StartTimerCommand command, CancellationToken cT)  // TODO: event handler?
    {
        command.Request.Deconstruct(
            out Guid roomId, 
            out Guid playerId);
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
                await _hubContext.Clients.User(playerId.ToString()).ReceiveUser_ChangedTimer(seconds, cT);
                await Task.Delay(1000, cT);
                
                _logger.LogDebug($"Time has passed: {seconds}");
            }
            await _hubContext.Clients.User(playerId.ToString()).ReceiveUser_TimeOut(cT);  
            _logger.LogInformation("en!! StartTimer end of work.");  // TODO finish log

            return false;
        }, cT);

        await Task.Delay(10, cT);
        return false;
    }
}

