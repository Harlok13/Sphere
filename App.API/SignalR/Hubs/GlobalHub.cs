using App.Contracts.Requests;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Sphere.SignalR.Commands.CreateRoom;
using Sphere.SignalR.Commands.JoinToRoom;

namespace Sphere.SignalR.Hubs;

// public class GlobalHub : Hub
public class GlobalHub : Hub<IGlobalHub>
{
    private readonly ILogger<GlobalHub> _logger;
    private readonly IMediator _mediator;

    public GlobalHub(ILogger<GlobalHub> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task GetTest(Guid id)
    {
        // Guid gid = Guid.Parse(id);
        await Clients.All.Test($"hello - {id}");
        _logger.LogInformation($"Invoke method \"CreateRoom\"");
    }

    public async Task CreateRoom(RoomRequest roomRequest, Guid playerId, string playerName)
    {
        var command = new CreateRoomCommand(roomRequest, playerId, playerName, Context.ConnectionId);
        await _mediator.Send(command);
    }

    public async Task JoinToRoom(Guid roomId, Guid playerId, string playerName)
    {
        _logger.LogInformation($"Invoke method \"JoinToRoom\"");
        var command = new JoinToRoomCommand(roomId, playerId, playerName,  Context.ConnectionId);
        await _mediator.Send(command);
    }
}