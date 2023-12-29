using App.Application.Repositories.UnitOfWork;
using App.Contracts.Requests;
using App.Contracts.Responses;
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

        var room = await _unitOfWork.RoomRepository.GetRoomByIdAsNoTrackingAsync(roomId, cT);

        var initiator = room.Players.Single(p => p.Id == initiatorId);
        if (!initiator.IsLeader)
        {
            await _publisher.Publish(new NotificationEvent(
                    NotificationText: NotificationMessages.KickPlayerFromRoom.NotLeader(),
                    TargetConnectionId: initiator.ConnectionId),
                cT);

            return false;
        }

        var kickedPlayer = room.Players.Single(p => p.Id == kickedPlayerId);
        if (kickedPlayer.InGame)
        {
            await _publisher.Publish(new NotificationEvent(
                    NotificationText: NotificationMessages.KickPlayerFromRoom.PlayerInGame(kickedPlayer.PlayerName),
                    TargetConnectionId: kickedPlayer.ConnectionId),
                cT);

            return false;
        }

        var request = new RemoveFromRoomRequest(RoomId: room.Id, PlayerId: kickedPlayer.Id);
        var response = await _mediator.Send(new RemoveFromRoomCommand(request, kickedPlayer.ConnectionId), cT);

        if (response)
        {
            await _publisher.Publish(new NotificationEvent(
                    NotificationText: NotificationMessages.KickPlayerFromRoom.SuccessKick(kickedPlayer.PlayerName),
                    TargetConnectionId: initiator.ConnectionId),
                cT);

            await _publisher.Publish(new NotificationEvent(
                    NotificationText: NotificationMessages.KickPlayerFromRoom.WasKicked(initiator.PlayerName),
                    TargetConnectionId: kickedPlayer.ConnectionId),
                cT);

            // room.AddKickedPlayer(kickedPlayerId);  // TODO: finish (attention, room as no tracking)
            return true;
        }

        _logger.LogError("Transaction failed when calling {Method}.", nameof(RemoveFromRoomCommand));
        await _publisher.Publish(new NotificationResponse(
                NotificationId: Guid.NewGuid(),
                NotificationText: "Something went wrong, please try again."),
            cT);

        return false;
    }
}
