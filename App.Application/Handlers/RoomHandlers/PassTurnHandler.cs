using System.Collections.Immutable;
using App.Application.Repositories;
using App.Application.Repositories.RoomRepository;
using App.Application.Repositories.UnitOfWork;
using App.Contracts.Requests;
using App.Domain.Enums;
using App.SignalR.Commands.RoomCommands;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.RoomHandlers;

public class PassTurnHandler : ICommandHandler<PassTurnCommand, bool>
{
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly IPublisher _publisher;
    private readonly ILogger<PassTurnHandler> _logger;
    private readonly IRoomRepository _roomRepository;
    private readonly IPlayerInfoRepository _playerInfoRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public PassTurnHandler(
        ILogger<PassTurnHandler> logger,
        IRoomRepository roomRepository,
        IPlayerInfoRepository playerInfoRepository,
        IAppUnitOfWork unitOfWork,
        IMediator mediator, 
        IPlayerRepository playerRepository,
        IHubContext<GlobalHub, IGlobalHub> hubContext,
        IPublisher publisher
    )
    {
        _logger = logger;
        _roomRepository = roomRepository;
        _playerInfoRepository = playerInfoRepository;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _playerRepository = playerRepository;
        _hubContext = hubContext;
        _publisher = publisher;
    }
    
    public async ValueTask<bool> Handle(PassTurnCommand command, CancellationToken cT)
    {
        command.Deconstruct(out Guid roomId);

        var room = await _unitOfWork.RoomRepository.GetByIdAsync(roomId, cT);
        var players = room.Players;

        // var playersInGame = players.Where(p => p.InGame).ToImmutableList().Count;
        // if (playersInGame < 2)
        // {
        //     // await _mediator.Send(new EndGameCommand(), cT);  TODO: finish
        // }


        if (players.Any(p => p.MoveStatus == MoveStatus.None))
        {
            players
                .FirstOrDefault(p => p.MoveStatus == MoveStatus.None)!
                .SetMove(true);

            // await _mediator.Send(new StartTimerCommand(new StartTimerRequest(room.Id, playerId), connectionId), cT);
            
            return await _unitOfWork.SaveChangesAsync(cT);
        }

        await _mediator.Send(new EndCycleCommand(RoomId: roomId), cT);  // TODO: finish
        return false;
    }
}

// EndCycleCommand
// StartCycleCommand
// EndGameCommand