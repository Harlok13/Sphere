using App.SignalR.Commands.RoomCommands;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.RoomHandlers;

public class StopTimerHandler : ICommandHandler<StopTimerCommand, bool>
{
    private readonly ILogger<StopTimerHandler> _logger;
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly IDistributedCache _cache;

    public StopTimerHandler(
        ILogger<StopTimerHandler> logger,
        IHubContext<GlobalHub, IGlobalHub> hubContext,
        IDistributedCache cache)
    {
        _logger = logger;
        _hubContext = hubContext;
        _cache = cache;
    }
    
    public async ValueTask<bool> Handle(StopTimerCommand command, CancellationToken cT)
    {
        // command.Deconstruct(out Guid roomId, out Guid userId, out string connectionId, out CancellationTokenSource cts);
        var cts = command.Cts;

        cts.Cancel();
        return true;
    }
}