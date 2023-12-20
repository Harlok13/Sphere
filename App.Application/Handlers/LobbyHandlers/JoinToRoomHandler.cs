using App.Application.Mapper;
using App.Application.Repositories;
using App.Application.Repositories.RoomRepository;
using App.Application.Repositories.UnitOfWork;
using App.SignalR.Commands;
using App.SignalR.Commands.LobbyCommands;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Player = App.Domain.Entities.PlayerEntity.Player;

namespace App.Application.Handlers.LobbyHandlers;

public class JoinToRoomHandler : ICommandHandler<JoinToRoomCommand, bool>
{
    // private readonly IGlobalHubContext _hubContext;
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    // private readonly IHubCallerClients<GlobalHub> _hubCaller;
    private readonly ILogger<JoinToRoomHandler> _logger;
    private readonly IRoomRepository _roomRepository;
    private readonly IPlayerInfoRepository _playerInfoRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public JoinToRoomHandler(
        // IGlobalHubContext hubContext,
        ILogger<JoinToRoomHandler> logger,
        IRoomRepository roomRepository,
        IPlayerInfoRepository playerInfoRepository,
        IAppUnitOfWork unitOfWork,
        IMediator mediator, 
        IPlayerRepository playerRepository,
        IHubContext<GlobalHub, IGlobalHub> hubContext
        // IHubCallerClients<GlobalHub> hubCaller
        )
    {
        // _hubContext = hubContext;
        _logger = logger;
        _roomRepository = roomRepository;
        _playerInfoRepository = playerInfoRepository;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _playerRepository = playerRepository;
        _hubContext = hubContext;
        // _hubCaller = hubCaller;
    }
    
    public async ValueTask<bool> Handle(JoinToRoomCommand command, CancellationToken cT)
    {
        command.Deconstruct(
            out Guid roomId,
            out Guid playerId,
            // out string playerName,
            out int selectedStartMoney,
            out string connectionId);
        
        var playerInfo = await _playerInfoRepository.GetPlayerInfoByIdAsync(playerId, cT);
        var room = await _roomRepository.GetRoomByIdAsync(roomId, cT);
        // TODO: if players in room < room size (validation)
        
        playerInfo.DecrementMoney(selectedStartMoney);
        await _hubContext.Clients.Client(connectionId).ReceiveOwn_UpdatedMoney(playerInfo.Money, cT);
        // var player = Player.Create(playerId, playerName, isLeader, roomId);
        var player = Player.Create(
            id: playerId,
            playerName: playerInfo.PlayerName,
            roomId: roomId,
            money: selectedStartMoney,
            connectionId: connectionId);
        // player.NotifyReceiveNewCard += CardHandler;
        if (room.Players.Count < 1) player.SetIsLeader(true);
        
        // await _playerRepository.CreatePlayerAsync(player, cT);

        room.AddNewPlayer(player);

        var roomInLobbyResponse = RoomMapper.MapRoomToRoomInLobbyResponse(room);
        var playerResponse = PlayerMapper.MapPlayerToPlayerResponse(player);
        var playersResponse = PlayerMapper.MapManyPlayersToManyPlayersResponse(room.Players);

        // if (room.Players.Count == 1) await _hubContext.Clients.All.ReceiveAll_NewRoom(roomInLobbyResponse, cT);
        await _hubContext.Groups.AddToGroupAsync(connectionId, room.Id.ToString(), cT);
        await _hubContext.Clients.Group(roomId.ToString()).ReceiveGroup_NewPlayer(playerResponse, cT);
        // await _hubContext.Clients.Group(roomId.ToString()).ReceiveUpdatedPlayersList(playersResponse, cT);
        await _hubContext.Clients.All.ReceiveAll_UpdatedRoom(roomInLobbyResponse, cT);

        var initRoomDataResponse = RoomMapper.MapRoomToInitRoomDataResponse(room);
        await _hubContext.Clients.Client(connectionId).ReceiveOwn_PlayerData(playerResponse, initRoomDataResponse, playersResponse, cT);

        await _unitOfWork.SaveChangesAsync(cT);

        return true;
    }

    // private async Task CardHandler(Player player, PlayerCardsEventArgs e, CancellationToken cT)
    // {
    //     await _hubContext.Clients.Group(player.RoomId.ToString()).ReceiveGroup_NewCard(e.Card, cT);
    //     // _hubContext.Clients.Client(player.ConnectionId).ReceiveOwn_Card();
    // }
}
