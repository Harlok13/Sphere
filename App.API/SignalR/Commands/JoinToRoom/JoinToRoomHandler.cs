using App.Application.Mapper;
using App.Application.Repositories;
using App.Application.Repositories.UnitOfWork;
using App.Application.SignalR.Hubs;
using App.Domain.Entities;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Sphere.SignalR.Hubs;

namespace Sphere.SignalR.Commands.JoinToRoom;

public class JoinToRoomHandler : ICommandHandler<JoinToRoomCommand, bool>
{
    // private readonly IGlobalHubContext _hubContext;
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    // private readonly IHubCallerClients<GlobalHub> _hubCaller;
    private readonly ILogger<JoinToRoomHandler> _logger;
    private readonly IRoomRepository _roomRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public JoinToRoomHandler(
        // IGlobalHubContext hubContext,
        ILogger<JoinToRoomHandler> logger,
        IRoomRepository roomRepository,
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
            out string playerName,
            out string connectionId,
            out bool isLeader);
        
        var room = await _roomRepository.GetRoomByIdAsync(roomId, cT);
        var player = Player.Create(playerId, playerName, isLeader, roomId);  // TODO: mistake with fields 
        await _playerRepository.CreatePlayerAsync(player, cT);

        room.AddNewPlayer(player);

        var roomInLobbyResponse = RoomMapper.MapRoomToRoomInLobbyResponse(room);
        var playerResponse = PlayerMapper.MapPlayerToPlayerResponse(player);
        var playersResponse = PlayerMapper.MapManyPlayersToManyPlayersResponse(room.Players);
  
        await _hubContext.Groups.AddToGroupAsync(connectionId, room.Id.ToString(), cT);
        await _hubContext.Clients.Group(roomId.ToString()).ReceiveNewPlayer(playerResponse, cT);
        // await _hubContext.Clients.Group(roomId.ToString()).ReceiveUpdatedPlayersList(playersResponse, cT);
        await _hubContext.Clients.All.ReceiveUpdatedRoom(roomInLobbyResponse, cT);

        await _hubContext.Clients.Client(connectionId).ReceiveOwnPlayerData(playerResponse, roomInLobbyResponse, playersResponse, cT);

        await _unitOfWork.SaveChangesAsync(cT);

        return true;
    }
}
