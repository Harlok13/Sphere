using App.Application.Mapper;
using App.Application.Repositories;
using App.Application.Repositories.RoomRepository;
using App.Application.Repositories.UnitOfWork;
using App.Application.Services.Interfaces;
using App.Domain.Entities;
using App.Domain.Entities.RoomEntity;
using App.SignalR.Commands;
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
    private readonly IGame21Service _game21Service;
    private readonly ICardsDeckService _cardsDeckService;

    public StartGameHandler(
        ILogger<StartGameHandler> logger,
        IPlayerRepository playerRepository,
        IRoomRepository roomRepository,
        IAppUnitOfWork unitOfWork,
        IHubContext<GlobalHub, IGlobalHub> hubContext,
        IGame21Service game21Service, 
        ICardsDeckService cardsDeckService)
    {
        _logger = logger;
        _playerRepository = playerRepository;
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
        _hubContext = hubContext;
        _game21Service = game21Service;
        _cardsDeckService = cardsDeckService;
    }
    
    public async ValueTask<bool> Handle(StartGameCommand command, CancellationToken cT)
    {
        command.Deconstruct(out Guid roomId, out Guid playerId);

        var room = await _roomRepository.GetRoomByIdAsync(roomId, cT);
        room.Notify += NotifyBankChanged;
        var startGameResult = room.StartGame(playerId);

        if (startGameResult.CanStart)
        {
            await _hubContext.Clients.Group(roomId.ToString()).ReceiveGroup_StartGame(cT);  // TODO: add game history msg
            foreach (var player in room.Players)
            {
                player.SetInGame(true);
                player.DecreaseMoney(room.StartBid);
                await room.IncreaseBank(room.StartBid, cT);
                
                await _hubContext.Clients.Client(player.ConnectionId).ReceiveOwn_InGame(true, cT);
                // await _hubContext.Clients.Group(roomId.ToString()).ReceiveGroup_PlayerInGame(player.Id, true, cT);
                // var playerResponse = PlayerMapper.MapPlayerToPlayerResponse(player);
                // await _hubContext.Clients.Group(roomId.ToString()).ReceiveGroup_UpdatedPlayer(playerResponse, cT);
                
                var card = Card.Create(Guid.NewGuid(), player.Id, _cardsDeckService.GetNextCard());
                player.SetCard(card);
                var cardResponse = CardMapper.MapCardToCardResponse(card);
                // await _hubContext.Clients.Group(roomId.ToString()).ReceiveGroup_NewCard(cardResponse, cT);  // TODO: bcs updated player at least 
                await _hubContext.Clients.Client(player.ConnectionId).ReceiveOwn_Card(cardResponse, cT);
                await _hubContext.Clients.Client(player.ConnectionId).ReceiveOwn_UpdatedGameMoney(player.Money, cT);
                
                var playerResponse = PlayerMapper.MapPlayerToPlayerResponse(player);
                await _hubContext.Clients.Group(roomId.ToString()).ReceiveGroup_UpdatedPlayer(playerResponse, cT);
                
                await Task.Delay(1000, cT);
            }
            
            // await _hubContext.Clients.Group(roomId.ToString()).ReceiveGroup_UpdatedBank(room.Bank, cT);
            await _hubContext.Clients.Client(startGameResult.MovePlayerConnectionId!).ReceiveOwn_Move(cT);

            var roomInLobbyResponse = RoomMapper.MapRoomToRoomInLobbyResponse(room);
            await _hubContext.Clients.All.ReceiveAll_UpdatedRoom(roomInLobbyResponse, cT);

            await _unitOfWork.SaveChangesAsync(cT);

            return true;
        }

        await _hubContext.Clients.Group(roomId.ToString())
            .ReceiveGroup_StartGameErrorNotification(startGameResult.ErrorMsg, cT);
        // TODO: StartGameErrorNotificationResponse

        return false;
    }

    private async Task NotifyBankChanged(Room sender, Room.BankEventArgs e, CancellationToken cT)
    {
        await _hubContext.Clients.Group(sender.Id.ToString()).ReceiveGroup_UpdatedBank(e.Value, cT);
    }

}
