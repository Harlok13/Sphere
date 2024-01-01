using App.Contracts.Requests;
using App.SignalR.Commands.ConnectionCommands;
using App.SignalR.Commands.LobbyCommands;
using App.SignalR.Commands.RoomCommands;
using App.SignalR.Commands.RoomCommands.GameActionCommands;
using App.SignalR.Commands.RoomCommands.PlayerActionCommands;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.SignalR.Hubs;

[Authorize]
public class GlobalHub : Hub<IGlobalHub>
{
    private readonly ILogger<GlobalHub> _logger;
    private readonly IMediator _mediator;

    public GlobalHub(ILogger<GlobalHub> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    
    public override async Task OnConnectedAsync()
    {
        var user = Context.ToAuthUser();
        _logger.LogInformation(
            "User {UserName}: Connected To GlobalHub. ConnectionId: {ConnectionId}",
            user.UserName,
            Context.ConnectionId
        );

        await _mediator.Send(new ConnectPlayerCommand(user));
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var user = Context.ToAuthUser();
        _logger.LogInformation(
            "User {UserName}: Disconnected From GlobalHub. ConnectionId: {ConnectionId}",
            user.UserName,
            Context.ConnectionId
        );

        // TODO: add cts?
        await _mediator.Send(new DisconnectPlayerCommand(user));
        await base.OnDisconnectedAsync(exception);
    }
    
    public async ValueTask<bool> CreateRoom(CreateRoomRequest request)
    {
        var command = new CreateRoomCommand(
            Request: request,
            ConnectionId: Context.ConnectionId);
        return await _mediator.Send(command);
    }
    
    public async ValueTask<bool> JoinToRoom(JoinToRoomRequest request)
    {
        var command = new JoinToRoomCommand(
            Request: request,
            ConnectionId: Context.ConnectionId);
        return await _mediator.Send(command);
    }

    public async ValueTask<bool> RemoveFromRoom(RemoveFromRoomRequest request) =>
        await _mediator.Send(new RemoveFromRoomCommand(request));
    
    public async ValueTask<bool> SelectStartGameMoney(SelectStartGameMoneyRequest request) =>
        await _mediator.Send(new SelectStartGameMoneyCommand(request));

    public async ValueTask<bool> ToggleReadiness(ToggleReadinessRequest request) =>
        await _mediator.Send(new ToggleReadinessCommand(request));

    public async ValueTask<bool> StartGame(StartGameRequest request) =>
        await _mediator.Send(new StartGameCommand(request));

    public async ValueTask<bool> StartTimer(StartTimerRequest request)
    {
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
        var result = Context.Items.TryGetValue(Context.ConnectionId, out var cts);
        Context.Items.Remove(Context.ConnectionId);
        
        _logger.LogInformation("ResultT of getting cts is {ResultT}.", result);
        
        var command = new StopTimerCommand(
            Request: request,
            ConnectionId: Context.ConnectionId,
            Cts: (cts as CancellationTokenSource)!);
        return await _mediator.Send(command);
    }

    public async ValueTask<bool> Hit(HitRequest request) =>
        await _mediator.Send(new HitCommand(request));

    public async ValueTask<bool> Stay(StayRequest request) => 
        await _mediator.Send(new StayCommand(request));

    public async ValueTask<bool> ConfirmReconnectingToRoom(int c) =>
        await _mediator.Send(new ConfirmReconnectingToRoomCommand(Context.ToAuthUser()));

    public async ValueTask<bool> CancelReconnectingToRoom(int c) =>
        await _mediator.Send(new CancelReconnectingToRoomCommand(Context.ToAuthUser()));

    public async ValueTask<bool> KickPlayerFromRoom(KickPlayerFromRoomRequest request) =>
        await _mediator.Send(new KickPlayerFromRoomCommand(request));

    public async ValueTask<bool> TransferLeadership(TransferLeadershipRequest request) =>
        await _mediator.Send(new TransferLeadershipCommand(request));
}
