using App.Application.Repositories.UnitOfWork;
using App.Application.Services.Interfaces;
using App.Contracts.Requests;
using App.Domain.Entities.RoomEntity;
using App.SignalR.Commands.LobbyCommands;
using Mediator;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.LobbyHandlers;

public class CreateRoomHandler : ICommandHandler<CreateRoomCommand, bool>
{
    private readonly ILogger<CreateRoomHandler> _logger;
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly ICardsDeckService _cardsDeck;

    public CreateRoomHandler(
        ILogger<CreateRoomHandler> logger,
        IAppUnitOfWork unitOfWork,
        IMediator mediator, 
        ICardsDeckService cardsDeck)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _cardsDeck = cardsDeck;
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
        
        await _unitOfWork.RoomRepository.AddAsync(room, cT);

        var joinToRoomRequest = new JoinToRoomRequest(
            RoomId: room.Id,
            PlayerId: playerId,
            SelectedStartMoney: selectedStartMoney);

        await _unitOfWork.SaveChangesAsync(cT);

        await _cardsDeck.CreateAsync(room.Id, cT);

        var joinToRoomCommand = new JoinToRoomCommand(joinToRoomRequest, connectionId);
        return await _mediator.Send(joinToRoomCommand, cT);
    }
}