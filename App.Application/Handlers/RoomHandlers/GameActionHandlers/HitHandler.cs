using App.Application.Repositories;
using App.Application.Repositories.RoomRepository;
using App.Application.Repositories.UnitOfWork;
using App.Application.Services.Interfaces;
using App.Domain.Entities;
using App.SignalR.Commands.RoomCommands;
using App.SignalR.Commands.RoomCommands.GameActionCommands;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.RoomHandlers.GameActionHandlers;

public class HitHandler : ICommandHandler<HitCommand, bool>
{
    private readonly ILogger<HitHandler> _logger;
    private readonly IPlayerRepository _playerRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly IMediator _mediator;
    private readonly IDistributedCache _cache;
    private readonly ICardsDeckRepository _cardsDeckRepository;
    private readonly ICardsDeckService _cardsDeck;

    public HitHandler(
        ILogger<HitHandler> logger,
        IPlayerRepository playerRepository,
        IRoomRepository roomRepository,
        IAppUnitOfWork unitOfWork,
        IHubContext<GlobalHub, IGlobalHub> hubContext,
        IMediator mediator, 
        IDistributedCache cache, 
        ICardsDeckRepository cardsDeckRepository, ICardsDeckService cardsDeck)
    {
        _logger = logger;
        _playerRepository = playerRepository;
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
        _hubContext = hubContext;
        _mediator = mediator;
        _cache = cache;
        _cardsDeckRepository = cardsDeckRepository;
        _cardsDeck = cardsDeck;
    }
    
    public async ValueTask<bool> Handle(HitCommand command, CancellationToken cT)
    {
        command.Request.Deconstruct(out Guid roomId, out Guid playerId);

        var room = await _unitOfWork.RoomRepository.GetByIdAsync(roomId, cT);
        var player = room.Players.Single(p => p.Id == playerId);
        player.Hit();
        
        var card = Card.Create(Guid.NewGuid(), player.Id, await _cardsDeck.GetNextCardAsync(roomId, cT));  // TODO: mediatr?

        player.AddNewCard(card: card, delayMs: 0);

        await _unitOfWork.SaveChangesAsync(cT);
        
        return await _mediator.Send(new PassTurnCommand(roomId), cT);
    }
}