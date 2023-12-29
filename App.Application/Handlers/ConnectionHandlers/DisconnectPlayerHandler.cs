using App.Application.Extensions;
using App.Application.Repositories.UnitOfWork;
using App.Contracts.Data;
using App.Contracts.Requests;
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
            _logger.LogError("Unable to perform \"{CommandName}\" operation because connection id is null.",
                nameof(DisconnectPlayerCommand));

            return false;
        }

        var disconnectedPlayerNoTrack = await _unitOfWork.PlayerRepository.GetPlayerByIdAsNoTrackingAsync(authUser.Id, cT);
        if (!disconnectedPlayerNoTrack.TryFromResult(out PlayerDto? disconnectedPlayerDto, out _))
        {
            _logger.LogInformation("The player {Name} is not in the room.", authUser.UserName);
            return false;
        }

        var room = await _unitOfWork.RoomRepository.GetRoomByIdAsync(disconnectedPlayerDto!.RoomId, cT);
        
        var disconnectPlayerResult = room.DisconnectPlayer(disconnectedPlayerDto.Id);
        if (!disconnectPlayerResult.TryFromResult(out Room.DisconnectPlayerDto? disconnectData, out var errors))
        {
            return await SendSomethingWentWrongNotification(errors, authUser.ConnectionId, cT);
        }

        if (disconnectData!.NeedRemoveRoom)
        {
            var request = new RemoveFromRoomRequest(RoomId: room.Id, PlayerId: disconnectedPlayerDto.Id);
            await _mediator.Send(new RemoveFromRoomCommand(request, authUser.ConnectionId), cT);

            await _unitOfWork.RoomRepository.RemoveRoomAsync(room.Id, cT);
        }

        return await _unitOfWork.SaveChangesAsync(cT);
    }
    
    private async ValueTask<bool> SendSomethingWentWrongNotification(
        IEnumerable<Error> errors,
        string targetConnectionId,
        CancellationToken cT)
    {
        foreach (var error in errors) _logger.LogError(error.Message);
            
        await _publisher.Publish(new ClientNotificationEvent(
                NotificationText: NotificationMessages.SomethingWentWrong(),
                TargetConnectionId: targetConnectionId),
            cT);
            
        return false;
    }
}