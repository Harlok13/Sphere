using App.Contracts.Requests;
using App.SignalR.Commands.ConnectionCommands;
using App.SignalR.Commands.LobbyCommands;
using App.SignalR.Commands.RoomCommands;
using App.SignalR.Commands.RoomCommands.GameActionCommands;
using App.SignalR.Commands.RoomCommands.PlayerActionCommands;
using App.SignalR.Extensions;
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
            "User {UserName}: Connected To GlobalHub. UserId: {UserId}. ConnectionId: {ConnectionId}",
            user.UserName,
            user.Id,
            Context.ConnectionId
        );

        await _mediator.Send(new ConnectPlayerCommand(user));
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var user = Context.ToAuthUser();
        _logger.LogInformation(
            "User {UserName}: Disconnected From GlobalHub. UserId: {UserId}. ConnectionId: {ConnectionId}",
            user.UserName,
            user.Id,
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
        var user = Context.ToAuthUser();
        if (!Context.Items.TryGetValue(user.Id, out var cts2))
        {
            var cts = new CancellationTokenSource();
            Context.Items.TryAdd(user.Id, cts);
            var command = new StartTimerCommand(
                Request: request,
                Cts: cts);
            
            return await _mediator.Send(command);
        } 
        
        return await _mediator.Send(new StartTimerCommand(Request: request, Cts: cts2 as CancellationTokenSource));
    }

    public async ValueTask<bool> StopTimer(StopTimerRequest request)
    {
        var user = Context.ToAuthUser();
        var result = Context.Items.TryGetValue(user.Id, out var cts);
        if (!result)
        {
            _logger.LogInformation(
                "{InvokingMethod} - Cts is not found.",
                nameof(StopTimer));
            return false;
        }
        Context.Items.Remove(user.Id);
        
        _logger.LogInformation("Result of getting cts is {Result}.", result);
        
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
