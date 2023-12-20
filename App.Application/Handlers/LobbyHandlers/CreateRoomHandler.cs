using App.Application.Repositories.RoomRepository;
using App.Application.Repositories.UnitOfWork;
using App.Domain.Entities.RoomEntity;
using App.SignalR.Commands;
using App.SignalR.Commands.LobbyCommands;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.LobbyHandlers;

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
        
        _logger.LogInformation($"Room request\nMinBid: {requestRoom.MinBid}\nMaxBid: {requestRoom.MaxBid}");

        var room = Room.Create(
            id: Guid.NewGuid(),
            roomName: requestRoom.RoomName,  // TODO: valid roomName, must be unique
            roomSize: requestRoom.RoomSize,
            startBid: requestRoom.StartBid,
            minBid: requestRoom.MinBid,
            maxBid: requestRoom.MaxBid,
            avatarUrl: requestRoom.ImgUrl);

        var selectStartGameMoneyCommand =
            new SelectStartGameMoneyCommand(room.Id, userId, room.StartBid, room.MinBid, room.MaxBid, connectionId);
        var response = await _mediator.Send(selectStartGameMoneyCommand, cT);

        if (response)
        {
            await _roomRepository.AddNewRoomAsync(room, cT);
        
            await _unitOfWork.SaveChangesAsync(cT);
        }
        
        return true;
    }
}
