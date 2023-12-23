using App.Application.Repositories.RoomRepository;
using App.Application.Repositories.UnitOfWork;
using App.Contracts.Requests;
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
        command.Request.Deconstruct(
            out RoomRequest roomRequest,
            out Guid playerId,
            out int selectedStartMoney,
            out int upperBound,
            out int lowerBound);

        var connectionId = command.ConnectionId;

        var room = Room.Create(
            id: Guid.NewGuid(),
            roomName: roomRequest.RoomName, // TODO: valid roomName, must be unique
            roomSize: roomRequest.RoomSize,
            startBid: roomRequest.StartBid,
            minBid: roomRequest.MinBid,
            maxBid: roomRequest.MaxBid,
            avatarUrl: roomRequest.AvatarUrl,
            lowerStartMoneyBound: lowerBound,
            upperStartMoneyBound: upperBound);
        
        await _unitOfWork.RoomRepository.AddNewRoomAsync(room, cT);

        var joinToRoomRequest = new JoinToRoomRequest(
            RoomId: room.Id,
            PlayerId: playerId,
            SelectedStartMoney: selectedStartMoney);

        await _unitOfWork.SaveChangesAsync(cT);
        
        var joinToRoomCommand = new JoinToRoomCommand(joinToRoomRequest, connectionId);
        await _mediator.Send(joinToRoomCommand, cT);


        return true;
    }
}