using App.Application.Extensions;
using App.Application.Messages;
using App.Application.Repositories.UnitOfWork;
using App.Contracts.Requests;
using App.Domain.DomainResults;
using App.Domain.Entities;
using App.Domain.Entities.RoomEntity;
using App.Domain.Shared;
using App.SignalR.Commands.ConnectionCommands;
using App.SignalR.Commands.RoomCommands;
using App.SignalR.Events;
using Mediator;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.ConnectionHandlers;

public class DisconnectPlayerHandler : ICommandHandler<DisconnectPlayerCommand, bool>
{
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly ILogger<DisconnectPlayerHandler> _logger;
    private readonly IPublisher _publisher;
    private readonly IMediator _mediator;

    public DisconnectPlayerHandler(
        IAppUnitOfWork unitOfWork,
        ILogger<DisconnectPlayerHandler> logger,
        IPublisher publisher,
        IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _publisher = publisher;
        _mediator = mediator;
    }

    public async ValueTask<bool> Handle(DisconnectPlayerCommand command, CancellationToken cT)
    {
        command.Deconstruct(out AuthUser authUser);
        if (authUser.ConnectionId is null)
        {
            _logger.LogError(
                "Unable to perform \"{CommandName}\" operation because connection id is null.",
                nameof(DisconnectPlayerCommand));

            return false;
        }

        // var disconnectedPlayerNoTrack = await _unitOfWork.PlayerRepository.GetPlayerByIdAsNoTrackingAsync(authUser.Id, cT);
        // if (!disconnectedPlayerNoTrack.TryFromDomainResult(out PlayerDto? disconnectedPlayerDto, out _))
        if (!_unitOfWork.PlayerRepository.CheckPlayerExists(authUser.Id))
        {
            _logger.LogInformation(
                "The player \"{Name}\" is not in the room.", 
                authUser.UserName);
            return false;
        }

        // var roomResult = await _unitOfWork.RoomRepository.GetByIdAsync(disconnectedPlayerDto!.RoomId, cT);
        var roomResult = await _unitOfWork.RoomRepository.GetByPlayerIdAsync1(authUser.Id, cT);
        if (!roomResult.TryFromResult(out Room? room, out var errors))
        {
            return await SendSomethingWentWrongNotification(errors, authUser.ConnectionId, cT);
        }
        
        var disconnectPlayerResult = room!.DisconnectPlayer(authUser.Id);
        // if (!disconnectPlayerResult.TryFromDomainResult(out Room.DisconnectPlayerDto? disconnectData, out var errors))
        // {
        //     return await SendSomethingWentWrongNotification(errors, authUser.ConnectionId, cT);
        // }
        if (disconnectPlayerResult is DomainError disconnectPlayerError)
        {
            _logger.LogError(disconnectPlayerError.Reason);
            return await SendSomethingWentWrongNotification(null, authUser.ConnectionId, cT);
        }

        await _unitOfWork.SaveChangesAsync(cT);

        if (room.Players.Count == 1)  // TODO: finish
        {
            var request = new RemoveFromRoomRequest(RoomId: room.Id, PlayerId: authUser.Id);
            _logger.LogInformation("User {UserId}: Invoking command {CommandName} with argument {Argument}.",
                authUser.Id,
                nameof(RemoveFromRoomCommand),
                new {Request = request});
            return await _mediator.Send(new RemoveFromRoomCommand(request), cT);
        }
        // if (disconnectData!.NeedRemoveRoom)
        // {
        //
        //     await _unitOfWork.RoomRepository.RemoveAsync(room.Id, cT);
        // }
        return true;
    }
    
    private async ValueTask<bool> SendSomethingWentWrongNotification(
        IEnumerable<Error>? errors,
        string targetConnectionId,
        CancellationToken cT)
    {
        if (errors is not null)
            foreach (var error in errors) _logger.LogError(error.Message);
            
        await _publisher.Publish(new ClientNotificationEvent(
                NotificationText: NotificationMessages.SomethingWentWrong(),
                TargetConnectionId: targetConnectionId),
            cT);
            
        return false;
    }
}