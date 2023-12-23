using App.Contracts.Requests;
using App.SignalR.Commands.LobbyCommands;
using App.SignalR.Commands.RoomCommands;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.SignalR.Hubs;

public class GlobalHub : Hub<IGlobalHub>
{
    private readonly ILogger<GlobalHub> _logger;
    private readonly IMediator _mediator;

    public GlobalHub(ILogger<GlobalHub> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    public async ValueTask<bool> CreateRoom(CreateRoomRequest request)
    {
        _logger.LogInformation($"Invoke method \"CreateRoom\" by user {request.PlayerId}. Create room {request.RoomRequest.RoomName}");
        var command = new CreateRoomCommand(
            Request: request,
            ConnectionId: Context.ConnectionId);
        return await _mediator.Send(command);
    }
    
    public async ValueTask<bool> JoinToRoom(JoinToRoomRequest request)
    {
        _logger.LogInformation($"Invoke method \"JoinToRoom\" by user {request.PlayerId} in room {request.RoomId}.");
        var command = new JoinToRoomCommand(
            Request: request,
            ConnectionId: Context.ConnectionId);
        return await _mediator.Send(command);
    }

    public async ValueTask<bool> RemoveFromRoom(RemoveFromRoomRequest request)
    {
        _logger.LogInformation($"Invoke method \"RemoveFromRoom\" by user {request.PlayerId} in room {request.RoomId}.");
        var command = new RemoveFromRoomCommand(
            Request: request,
            ConnectionId: Context.ConnectionId);
        return await _mediator.Send(command);
    }

    public async ValueTask<bool> SelectStartGameMoney(SelectStartGameMoneyRequest request)
    {
        _logger.LogInformation($"Invoke method \"{nameof(SelectStartGameMoney)}\" by user {request.PlayerId}.");
        var command = new SelectStartGameMoneyCommand(
            Request: request,
            ConnectionId: Context.ConnectionId);
        return await _mediator.Send(command);
    }

    public async ValueTask<bool> ToggleReadiness(ToggleReadinessRequest request)
    {
        _logger.LogInformation($"Invoke method \"ToggleReadiness\" by user {request.PlayerId} in room {request.RoomId}.");
        var command = new ToggleReadinessCommand(
            Request: request);
        return await _mediator.Send(command);
    }

    public async ValueTask<bool> StartGame(StartGameRequest request)
    {
        _logger.LogInformation($"Invoke method \"StartGame\" by user {request.PlayerId} in room {request.RoomId}");
        var command = new StartGameCommand(Request: request);
        return await _mediator.Send(command);
    }

    public async ValueTask<bool> StartTimer(StartTimerRequest request)
    {
        _logger.LogInformation($"Invoke method \"StartTimer\" by user {request.PlayerId} in room {request.RoomId}");
        
        var cts = new CancellationTokenSource();
        Context.Items.TryAdd(Context.ConnectionId, cts);

        var command = new StartTimerCommand(
            Request: request,
            ConnectionId: Context.ConnectionId, 
            Cts: cts);
        return await _mediator.Send(command);
    }

    public async ValueTask<bool> StopTimer(StopTimerRequest request)
    {
        _logger.LogInformation($"Invoke method \"StopTimer\" by user {request.PlayerId} in room {request.RoomId}.");
        
        var result = Context.Items.TryGetValue(Context.ConnectionId, out var cts);
        Context.Items.Remove(Context.ConnectionId);
        
        _logger.LogInformation($"Result of getting cts is {result}. Cts is null? {cts is null}.");
        
        var command = new StopTimerCommand(
            Request: request,
            ConnectionId: Context.ConnectionId,
            Cts: (cts as CancellationTokenSource)!);
        return await _mediator.Send(command);
    }
}
