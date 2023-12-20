using App.Application.Mapper;
using App.Application.Repositories;
using App.Application.Repositories.RoomRepository;
using App.Application.Repositories.UnitOfWork;
using App.SignalR.Commands;
using App.SignalR.Commands.RoomCommands;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.RoomHandlers;

public class FoldHandler : ICommandHandler<FoldCommand, bool>
{
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly ILogger<FoldHandler> _logger;
    private readonly IRoomRepository _roomRepository;
    private readonly IPlayerInfoRepository _playerInfoRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public FoldHandler(
        ILogger<FoldHandler> logger,
        IRoomRepository roomRepository,
        IPlayerInfoRepository playerInfoRepository,
        IAppUnitOfWork unitOfWork,
        IMediator mediator, 
        IPlayerRepository playerRepository,
        IHubContext<GlobalHub, IGlobalHub> hubContext
    )
    {
        _logger = logger;
        _roomRepository = roomRepository;
        _playerInfoRepository = playerInfoRepository;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _playerRepository = playerRepository;
        _hubContext = hubContext;
    }
    
    public async ValueTask<bool> Handle(FoldCommand command, CancellationToken cT)
    {
        command.Deconstruct(out Guid roomId, out Guid playerId);

        var player = await _playerRepository.GetPlayerByIdAsync(playerId, cT);
        player.ResetCards();
        player.SetInGame(false);

        await _hubContext.Clients.Client(player.ConnectionId).ReceiveOwn_InGame(false, cT);
        // await _hubContext.Clients.Group(roomId.ToString()).ReceiveGroup_PlayerInGame(playerId, false, cT);
        var playerResponse = PlayerMapper.MapPlayerToPlayerResponse(player);
        await _hubContext.Clients.Group(roomId.ToString()).ReceiveGroup_UpdatedPlayer(playerResponse, cT);
        
        await _hubContext.Clients.Client(player.ConnectionId).ReceiveOwn_FoldCards(cT);
        await _hubContext.Clients.Group(roomId.ToString()).ReceiveGroup_PlayerFold(playerId, cT);

        await _unitOfWork.SaveChangesAsync(cT);
        return true;
    }
}