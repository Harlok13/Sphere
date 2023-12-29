using System.Collections.Immutable;
using App.Application.Repositories.RoomRepository;
using App.Application.Repositories.UnitOfWork;
using App.Domain.Entities;
using App.SignalR.Commands.ConnectionCommands;
using App.SignalR.Commands.LobbyCommands;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.ConnectionHandlers;

public class DisconnectPlayerHandler : ICommandHandler<DisconnectPlayerCommand, bool>
{
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly ILogger<DisconnectPlayerHandler> _logger;
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly IPublisher _publisher;
    private readonly IRoomRepository _roomRepository;
    private readonly IMediator _mediator;

    public DisconnectPlayerHandler(
        IAppUnitOfWork unitOfWork,
        ILogger<DisconnectPlayerHandler> logger,
        IHubContext<GlobalHub, IGlobalHub> hubContext,
        IPublisher publisher,
        IRoomRepository roomRepository, 
        IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _hubContext = hubContext;
        _publisher = publisher;
        _roomRepository = roomRepository;
        _mediator = mediator;
    }

    public async ValueTask<bool> Handle(DisconnectPlayerCommand command, CancellationToken cT)
    {
        command.Deconstruct(out IUser authUser);

        var disconnectedPlayer = await _unitOfWork.PlayerRepository.GetPlayerByIdAsync(authUser.Id, cT);
        if (disconnectedPlayer is null)
        {
            _logger.LogInformation("The player {Name} is not in the room.", authUser.UserName);
            return true;
        }

        disconnectedPlayer.SetOnline(false);

        var room = await _roomRepository.GetRoomByIdAsNoTrackingAsync(disconnectedPlayer.RoomId, cT);
        var playersOnline = room.Players
            .Where(p => p.Online && p.Id != disconnectedPlayer.Id)
            .ToImmutableArray();
        
        _logger.LogInformation(
            "Players online in room {RoomId}: {PlayerCount}",
            room.Id,
            playersOnline.Length);

        /* cuz the transaction has not been applied */
        if (playersOnline.Length < 1)
        {
            // await _mediator.Send(new DeadRoomCommand(), cT);
            // _ = Task.Run(async () =>
            // {
            //     await Task.Delay(300000);
            //     
            if (_roomRepository is RoomRepositoryNotifyDecorator rep)
            {
                rep.NotifyRemoveRoom += RemoveRoomAsync;
            }
            await _roomRepository.RemoveRoomAsync(room.Id, cT);
            _logger.LogInformation("");
            // });
        }

        // var room = await _unitOfWork.RoomRepository.GetRoomByIdAsNoTrackingAsync(disconnectedPlayer.RoomId, cT);
        // var players = room.Players;
        //
        // var playersExceptDisconnectedPlayerIds = players
        //     .Where(p => p.Id != disconnectedPlayer.Id && p.Online)
        //     .Select(p => p.Id.ToString())
        //     .ToImmutableArray();
        //
        // var response = new DisconnectedPlayerResponse(disconnectedPlayer.Id);
        //
        // await _hubContext.Clients
        //     .Users(playersExceptDisconnectedPlayerIds)
        //     .ReceiveGroup_DisconnectedPlayer(response, cT);
        // _logger.LogInformation(
        //     "");

        return await _unitOfWork.SaveChangesAsync(cT);
    }
    
    private async Task RemoveRoomAsync(RoomRepositoryNotifyDecorator.RemoveRoomEventArgs e, CancellationToken cT)
    {
        _logger.LogInformation("Receive remove room in event.");
        await _hubContext.Clients.All.ReceiveAll_RemovedRoom(e.RoomId, cT);
    }
}