using App.Application.Extensions;
using App.Application.Repositories.UnitOfWork;
using App.Contracts.Data;
using App.Contracts.Requests;
using App.Domain.Entities;
using App.SignalR.Commands.ConnectionCommands;
using App.SignalR.Commands.RoomCommands;
using App.SignalR.Events;
using Mediator;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.ConnectionHandlers;

public class CancelReconnectingToRoomHandler : ICommandHandler<CancelReconnectingToRoomCommand, bool>
{
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly ILogger<CancelReconnectingToRoomHandler> _logger;
    private readonly IMediator _mediator;
    private readonly IPublisher _publisher;

    public CancelReconnectingToRoomHandler(
        IAppUnitOfWork unitOfWork,
        ILogger<CancelReconnectingToRoomHandler> logger,
        IMediator mediator, 
        IPublisher publisher)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mediator = mediator;
        _publisher = publisher;
    }
    
    public async ValueTask<bool> Handle(CancelReconnectingToRoomCommand command, CancellationToken cT)
    {
        command.Deconstruct(out AuthUser authUser);
        if (authUser.ConnectionId is null)
        {
            _logger.LogError("Unable to perform \"{CommandName}\" operation because connection id is null.",
                nameof(DisconnectPlayerCommand));

            return false;
        }

        var playerResult = await _unitOfWork.PlayerRepository.GetPlayerByIdAsNoTrackingAsync(authUser.Id, cT);
        if (!playerResult.TryFromResult(out PlayerDto? playerNoTrack, out var errors))
        {
            foreach (var error in errors) _logger.LogError(error.Message);

            await _publisher.Publish(new UserNotificationEvent(
                    NotificationText: NotificationMessages.SomethingWentWrong(),
                    TargetId: authUser.Id),
                cT);

            return false;
        }
        
        var request = new RemoveFromRoomRequest(playerNoTrack!.RoomId, playerNoTrack.Id);
        return await _mediator.Send(new RemoveFromRoomCommand(request, authUser.ConnectionId), cT);
    }
}