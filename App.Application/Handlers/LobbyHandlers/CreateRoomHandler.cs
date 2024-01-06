using App.Application.Repositories.UnitOfWork;
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

    public CreateRoomHandler(
        ILogger<CreateRoomHandler> logger,
        IAppUnitOfWork unitOfWork,
        IMediator mediator)
    {
        _logger = logger;
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
            roomName: roomRequest.RoomName,
            roomSize: roomRequest.RoomSize,
            startBid: roomRequest.StartBid,
            minBid: roomRequest.MinBid,
            maxBid: roomRequest.MaxBid,
            avatarUrl: roomRequest.AvatarUrl,
            lowerStartMoneyBound: lowerBound,
            upperStartMoneyBound: upperBound);
        
        await _unitOfWork.RoomRepository.AddAsync(room, cT);
        
        await _unitOfWork.SaveChangesAsync(cT);
        
        var joinToRoomRequest = new JoinToRoomRequest(
            RoomId: room.Id,
            PlayerId: playerId,
            SelectedStartMoney: selectedStartMoney);
        
        _logger.LogInformation("User {UserId}: Invoking command {CommandName} with argument {Argument}.",
            playerId,
            nameof(CreateRoomCommand),
            new {Request = joinToRoomRequest, ConnectionId = connectionId});
        return await _mediator.Send(new JoinToRoomCommand(joinToRoomRequest, connectionId), cT);
    }
}