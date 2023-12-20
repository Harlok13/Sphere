using App.Contracts.Requests;
using App.SignalR.Commands;
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
    public async Task CreateRoom(RoomRequest roomRequest, Guid playerId, string playerName)
    {
        _logger.LogInformation($"Invoke method \"CreateRoom\" by user {playerId}. Create room {roomRequest.RoomName}");
        var command = new CreateRoomCommand(
            RoomRequest: roomRequest,
            PlayerId: playerId,
            PlayerName: playerName,
            ConnectionId: Context.ConnectionId);
        await _mediator.Send(command);
    }

    public async Task JoinToRoom(Guid roomId, Guid playerId, int selectedStartMoney)
    {
        _logger.LogInformation($"Invoke method \"JoinToRoom\" by user {playerId} in room {roomId}.");
        var command = new JoinToRoomCommand(
            RoomId: roomId,
            PlayerId: playerId,
            // PlayerName: playerName,
            SelectedStartMoney: selectedStartMoney,
            ConnectionId:Context.ConnectionId);
        await _mediator.Send(command);
    }

    public async Task RemoveFromRoom(Guid roomId, Guid playerId)
    {
        _logger.LogInformation($"Invoke method \"RemoveFromRoom\" by user {playerId} in room {roomId}.");
        var command = new RemoveFromRoomCommand(
            RoomId: roomId,
            PlayerId: playerId,
            ConnectionId: Context.ConnectionId);
        await _mediator.Send(command);
    }

    public async Task SelectStartGameMoney(Guid roomId, Guid playerId, int startBid, int minBid, int maxBid)
    {
        var command = new SelectStartGameMoneyCommand(
            RoomId: roomId,
            PlayerId: playerId,
            ConnectionId: Context.ConnectionId,
            StartBid: startBid,
            MinBid: minBid,
            MaxBid: maxBid);
        await _mediator.Send(command);
    }

    public async Task ToggleReadiness(Guid roomId, Guid playerId)
    {
        _logger.LogInformation($"Invoke method \"ToggleReadiness\" by user {playerId} in room {roomId}.");
        var command = new ToggleReadinessCommand(
            RoomId: roomId, 
            PlayerId: playerId,
            ConnectionId: Context.ConnectionId);
        await _mediator.Send(command);
    }

    public async Task StartGame(Guid roomId, Guid playerId)
    {
        _logger.LogInformation($"Invoke method \"StartGame\" by user {playerId} in room {roomId}");
        var command = new StartGameCommand(RoomId: roomId, PlayerId: playerId);
        await _mediator.Send(command);
    }

    public async Task StartTimer(Guid roomId, Guid playerId)
    {
        _logger.LogInformation($"Invoke method \"StartTimer\" by user {playerId} in room {roomId}");
        
        var cts = new CancellationTokenSource();
        Context.Items.TryAdd(Context.ConnectionId, cts);

        var command = new StartTimerCommand(
            RoomId: roomId,
            PlayerId: playerId,
            ConnectionId: Context.ConnectionId, 
            Cts: cts);
        await _mediator.Send(command);
    }

    public async Task StopTimer(Guid roomId, Guid playerId)
    {
        _logger.LogInformation($"Invoke method \"StopTimer\" by user {playerId} in room {roomId}.");
        var result = Context.Items.TryGetValue(Context.ConnectionId, out var cts);
        Context.Items.Remove(Context.ConnectionId);
        
        _logger.LogInformation($"Result of getting cts is {result}. Cts is null? {cts is null}.");
        
        var command = new StopTimerCommand(
            RoomId: roomId,
            PlayerId: playerId,
            ConnectionId: Context.ConnectionId,
            Cts: (cts as CancellationTokenSource)!);
        await _mediator.Send(command);
    }
}
