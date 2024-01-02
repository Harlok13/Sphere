using App.Application.Extensions;
using App.Application.Repositories.UnitOfWork;
using App.Contracts.Requests;
using App.Domain.DomainResults;
using App.Domain.Entities.RoomEntity;
using App.SignalR.Commands.RoomCommands;
using App.SignalR.Commands.RoomCommands.PlayerActionCommands;
using App.SignalR.Events;
using Mediator;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.RoomHandlers.PlayerActionHandlers;

public class KickPlayerFromRoomHandler : ICommandHandler<KickPlayerFromRoomCommand, bool>
{
    private readonly ILogger<KickPlayerFromRoomHandler> _logger;
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IPublisher _publisher;

    public KickPlayerFromRoomHandler(
        ILogger<KickPlayerFromRoomHandler> logger,
        IAppUnitOfWork unitOfWork,
        IMediator mediator,
        IPublisher publisher)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _publisher = publisher;
    }

    /* the return value is responsible for closing the modal window. true - close the window, false - leave it open */
    public async ValueTask<bool> Handle(KickPlayerFromRoomCommand command, CancellationToken cT)
    {
        command.Request.Deconstruct(out Guid kickedPlayerId, out Guid initiatorId, out Guid roomId);

        var roomResult = await _unitOfWork.RoomRepository.GetByIdAsync(roomId, cT);
        if (!roomResult.TryFromResult(out Room? room, out var roomErrors))
        {
            foreach(var error in roomErrors) _logger.LogError(error.Message);

            await _publisher.Publish(new UserNotificationEvent(
                    NotificationText: NotificationMessages.SomethingWentWrong(),
                    TargetId: initiatorId),
                cT);

            return false;
        }

        // var initiator = room.Players.Single(p => p.Id == initiatorId);
        // if (!initiator.IsLeader)
        // {
        //     await _publisher.Publish(new ClientNotificationEvent(
        //             NotificationText: NotificationMessages.KickPlayerFromRoom.NotLeader(),
        //             TargetConnectionId: initiator.ConnectionId),
        //         cT);
        //
        //     return false;
        // }
        //
        // var kickedPlayer = room.Players.Single(p => p.Id == kickedPlayerId);
        // if (kickedPlayer.InGame)
        // {
        //     await _publisher.Publish(new ClientNotificationEvent(
        //             NotificationText: NotificationMessages.KickPlayerFromRoom.PlayerInGame(kickedPlayer.PlayerName),
        //             TargetConnectionId: kickedPlayer.ConnectionId),
        //         cT);
        //
        //     return false;
        // }
        
        /*
         if the transaction fails, the changes will not be saved
         so as long as the player is not deleted, we can get his data
        */
        var kickPlayerResult = room!.KickPlayer(initiatorId: initiatorId, kickedId: kickedPlayerId);
        if (kickPlayerResult is DomainFailure kickPlayerFailure)
        {
            await _publisher.Publish(new UserNotificationEvent(
                    NotificationText: kickPlayerFailure.Reason,
                    TargetId: initiatorId),
                cT);

            return false;
        }
        if (kickPlayerResult is DomainError kickPlayerError)
        {
            _logger.LogError(kickPlayerError.Reason);

            await _publisher.Publish(new UserNotificationEvent(
                    NotificationText: NotificationMessages.SomethingWentWrong(),
                    TargetId: initiatorId),
                cT);

            return false;
        }

        await _unitOfWork.SaveChangesAsync(cT);  // TODO: what to do with the transaction?

        var request = new RemoveFromRoomRequest(RoomId: room.Id, PlayerId: kickedPlayerId);
        _logger.LogInformation("User {UserId}: Invoking command {CommandName} with argument {Argument}.",
            initiatorId,
            nameof(RemoveFromRoomCommand),
            new {Request = request});
        var response = await _mediator.Send(new RemoveFromRoomCommand(request), cT);

        // if (response)
        // {
        //     
        //     await _publisher.Publish(new ClientNotificationEvent(
        //             NotificationText: NotificationMessages.KickPlayerFromRoom.SuccessKick(kickedPlayer.PlayerName),
        //             TargetConnectionId: initiator.ConnectionId),
        //         cT);
        //
        //     await _publisher.Publish(new ClientNotificationEvent(
        // NotificationText: NotificationMessages.KickPlayerFromRoom.WasKicked(initiator.PlayerName),
        //             TargetConnectionId: kickedPlayer.ConnectionId),
        //         cT);
        //
        //     // room.AddKickedPlayer(kickedPlayerId);  // TODO: finish (attention, room as no tracking)
        //     return true;
        // }
        if (response)
        {
            _logger.LogError("Transaction failed when calling {Method}.", nameof(RemoveFromRoomCommand));
            await _publisher.Publish(new UserNotificationEvent(
                    NotificationText: NotificationMessages.SomethingWentWrong(),
                    TargetId: initiatorId),
                cT);

            return false;
        }

        return response;
    }
}
