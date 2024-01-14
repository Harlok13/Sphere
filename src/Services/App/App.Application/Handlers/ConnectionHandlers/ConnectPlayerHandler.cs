using App.Application.Extensions;
using App.Application.Repositories.UnitOfWork;
using App.Contracts.Data;
using App.Contracts.Enums;
using App.Domain.Entities;
using App.Domain.Entities.RoomEntity;
using App.SignalR.Commands.ConnectionCommands;
using App.SignalR.Events;
using Mediator;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.ConnectionHandlers;

public class ConnectPlayerHandler : ICommandHandler<ConnectPlayerCommand, bool>
{
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly ILogger<ConnectPlayerHandler> _logger;
    private readonly IPublisher _publisher;
    private readonly IMediator _mediator;

    public ConnectPlayerHandler(
        IAppUnitOfWork unitOfWork,
        ILogger<ConnectPlayerHandler> logger,
        IPublisher publisher, 
        IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _publisher = publisher;
        _mediator = mediator;
    }

    public async ValueTask<bool> Handle(ConnectPlayerCommand command, CancellationToken cT)
    {
        command.Deconstruct(out AuthUser authUser);

        if (!_unitOfWork.PlayerRepository.CheckPlayerExists(authUser.Id))
        {
            _logger.LogInformation(
                "The player \"{Name}\" is not in the room.",
                authUser.UserName);
            return false;
        }

        await _publisher.Publish(new UserNavigateEvent(
                TargetId: authUser.Id,
                Navigate: NavigateEnum.Lobby),
            cT);

        // var roomIdResult = await _unitOfWork.RoomRepository.GetIdByPlayerIdAsync(authUser.Id, cT);
        // if (!roomIdResult.TryFromResult(out RoomIdDto? data, out var errors))
        // {
        //     foreach(var error in errors) _logger.LogError(error.Message);
        //     return false;
        // }
        
        var roomResult = await _unitOfWork.RoomRepository.GetByPlayerIdAsync1(authUser.Id, cT);  // TODO: ref, use dto
        if (!roomResult.TryFromResult(out Room? room, out var errors))
        {
            foreach(var error in errors) _logger.LogError(error.Message);
            return false;
        }

        if (room.Players.SingleOrDefault(p => p.Id == authUser.Id).InGame)
        {
            _logger.LogInformation("");  // TODO: log
            return await _mediator.Send(new ConfirmReconnectingToRoomCommand(authUser), cT);
        }
        
        // await _publisher.Publish(new UserReconnectToRoomEvent(
        //         RoomId: data!.RoomId,
        //         PlayerId: authUser.Id),
        //     cT);
        
        await _publisher.Publish(new UserReconnectToRoomEvent(
                RoomId: room.Id,
                PlayerId: authUser.Id),
            cT);

        return true;
    }
}