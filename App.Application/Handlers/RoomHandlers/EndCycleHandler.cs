using App.Application.Repositories;
using App.Application.Repositories.RoomRepository;
using App.Application.Repositories.UnitOfWork;
using App.Domain.Enums;
using App.SignalR.Commands.RoomCommands;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.RoomHandlers;

public class EndCycleHandler : ICommandHandler<EndCycleCommand, bool>
{
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly IPublisher _publisher;
    private readonly ILogger<EndCycleHandler> _logger;
    private readonly IRoomRepository _roomRepository;
    private readonly IPlayerInfoRepository _playerInfoRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public EndCycleHandler(
        ILogger<EndCycleHandler> logger,
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
    
    public async ValueTask<bool> Handle(EndCycleCommand command, CancellationToken cT)
    {
        command.Deconstruct(out Guid roomId);

        var room = await _unitOfWork.RoomRepository.GetByIdAsync(roomId, cT);
        var players = room.Players;

        if (players.All(p => p.MoveStatus == MoveStatus.Stay))
        {
            return await _mediator.Send(new EndGameCommand(roomId), cT);
        }

        var cPlayers = players.Where(p => p.MoveStatus != MoveStatus.Stay);
        foreach (var player in cPlayers)
        {
            player.SetMoveStatus(MoveStatus.None);
        }

        // TODO: check cards count and MoveStatus
        await _unitOfWork.SaveChangesAsync(cT);
        return await _mediator.Send(new PassTurnCommand(roomId), cT);
    }
}