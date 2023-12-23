using App.Application.Repositories;
using App.Application.Repositories.RoomRepository;
using App.Application.Repositories.UnitOfWork;
using App.Application.Services.Interfaces;
using App.Contracts.Mapper;
using App.Domain.Entities;
using App.SignalR.Commands.RoomCommands;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.RoomHandlers;

public class StartGameHandler : ICommandHandler<StartGameCommand, bool>
{
    private readonly ILogger<StartGameHandler> _logger;
    private readonly IPlayerRepository _playerRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly ICardsDeckService _cardsDeckService;

    public StartGameHandler(
        ILogger<StartGameHandler> logger,
        IPlayerRepository playerRepository,
        IRoomRepository roomRepository,
        IAppUnitOfWork unitOfWork,
        IHubContext<GlobalHub, IGlobalHub> hubContext,
        ICardsDeckService cardsDeckService)
    {
        _logger = logger;
        _playerRepository = playerRepository;
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
        _hubContext = hubContext;
        _cardsDeckService = cardsDeckService;
    }
    
    public async ValueTask<bool> Handle(StartGameCommand command, CancellationToken cT)
    {
        command.Request.Deconstruct(out Guid roomId, out Guid playerId);

        var room = await _unitOfWork.RoomRepository.GetRoomByIdAsync(roomId, cT);
        var startGameResult = room.StartGame(playerId);

        if (startGameResult.CanStart)
        {
            await _hubContext.Clients.Group(roomId.ToString()).ReceiveGroup_StartGame(cT);  // TODO: add game history msg
            
            foreach ( var indexAkaDelay in Enumerable.Range(0, room.Players.Count))
            {
                var player = room[indexAkaDelay];
                player.SetInGame(true);
                player.DecreaseMoney(room.StartBid);
                room.IncreaseBank(room.StartBid);
                
                var card = Card.Create(Guid.NewGuid(), player.Id, _cardsDeckService.GetNextCard());  // TODO: mediatr?

                var delayMs = indexAkaDelay * 1000;
                player.AddNewCard(card: card, delayMs: delayMs);
            }

            return await _unitOfWork.SaveChangesAsync(cT);
        }

        await _hubContext.Clients.Group(roomId.ToString())
            .ReceiveGroup_StartGameErrorNotification(startGameResult.ErrorMsg, cT);
        // TODO: StartGameErrorNotificationResponse

        return false;
    }
}
