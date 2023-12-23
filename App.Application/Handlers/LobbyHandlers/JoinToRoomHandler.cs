using App.Application.Repositories;
using App.Application.Repositories.RoomRepository;
using App.Application.Repositories.UnitOfWork;
using App.SignalR.Commands.LobbyCommands;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Player = App.Domain.Entities.PlayerEntity.Player;

namespace App.Application.Handlers.LobbyHandlers;

public class JoinToRoomHandler : ICommandHandler<JoinToRoomCommand, bool>
{
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly ILogger<JoinToRoomHandler> _logger;
    private readonly IRoomRepository _roomRepository;
    private readonly IPlayerInfoRepository _playerInfoRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public JoinToRoomHandler(
        ILogger<JoinToRoomHandler> logger,
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
    
    public async ValueTask<bool> Handle(JoinToRoomCommand command, CancellationToken cT)
    {
        command.Request.Deconstruct(
            out Guid roomId, out Guid playerId, out int selectedStartMoney);

        var connectionId = command.ConnectionId;
        
        var room = await _roomRepository.GetRoomByIdAsync(roomId, cT);

        if (room.Players.Count == room.RoomSize)
        {
            // TODO: room is full message (_hubContext.Clients.Client(player.ConnectionId).ReceiveOwn_RoomIsFullNotification());
            return false;
        }
    
        var playerInfo = await _playerInfoRepository.GetPlayerInfoByIdAsync(playerId, cT);
        
        playerInfo.DecrementMoney(selectedStartMoney, connectionId);

        var isLeader = room.Players.Count < 1;
        var player = Player.Create(
            id: playerId,
            playerName: playerInfo.PlayerName,
            roomId: roomId,
            money: selectedStartMoney,
            connectionId: connectionId,
            isLeader: isLeader,
            room: room);
        _logger.LogInformation($"room is null? {player.Room is null}");
        room.AddNewPlayer(player);

        return await _unitOfWork.SaveChangesAsync(cT);
    }
}
