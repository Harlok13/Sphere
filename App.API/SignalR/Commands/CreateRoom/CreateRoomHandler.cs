using App.Application.Mapper;
using App.Application.Repositories;
using App.Application.Repositories.UnitOfWork;
using App.Application.SignalR.Hubs;
using App.Domain.Entities;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using Sphere.SignalR.Commands.JoinToRoom;
using Sphere.SignalR.Hubs;

namespace Sphere.SignalR.Commands.CreateRoom;

public class CreateRoomHandler : ICommandHandler<CreateRoomCommand, bool>
{
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly ILogger<CreateRoomHandler> _logger;
    private readonly IRoomRepository _roomRepository;
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public CreateRoomHandler(
        IHubContext<GlobalHub, IGlobalHub> hubContext,
        ILogger<CreateRoomHandler> logger,
        IRoomRepository roomRepository,
        IAppUnitOfWork unitOfWork,
        IMediator mediator
        )
    {
        _hubContext = hubContext;
        _logger = logger;
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }
    
    public async ValueTask<bool> Handle(CreateRoomCommand command, CancellationToken cT)
    {
        var (requestRoom, userId, userName, connectionId, isLeader) = command;

        var room = Room.Create(
            Guid.NewGuid(),
            requestRoom.RoomName,
            requestRoom.RoomSize,
            requestRoom.StartBid,
            requestRoom.MaxBid,
            requestRoom.MinBid,
            requestRoom.ImgUrl);

        await _roomRepository.AddNewRoomAsync(room, cT);
        await _unitOfWork.SaveChangesAsync(cT);  // TODO remove?
        
        var roomInLobbyResponse = RoomMapper.MapRoomToRoomInLobbyResponse(room);
        await _hubContext.Clients.All.ReceiveNewRoom(roomInLobbyResponse, cT);
        
        var comm = new JoinToRoomCommand(room.Id, userId, userName, connectionId, IsLeader: isLeader);
        await _mediator.Send(comm, cT);
        
        return true;
    }
}
