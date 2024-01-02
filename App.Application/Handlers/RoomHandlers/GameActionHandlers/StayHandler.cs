using App.Application.Extensions;
using App.Application.Repositories.UnitOfWork;
using App.Domain.DomainResults;
using App.Domain.Entities.RoomEntity;
using App.SignalR.Commands.RoomCommands;
using App.SignalR.Commands.RoomCommands.GameActionCommands;
using App.SignalR.Events;
using Mediator;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.RoomHandlers.GameActionHandlers;

public class StayHandler : ICommandHandler<StayCommand, bool>
{
    private readonly ILogger<StayHandler> _logger;
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IPublisher _publisher;

    public StayHandler(
        ILogger<StayHandler> logger,
        IAppUnitOfWork unitOfWork,
        IMediator mediator,
        IPublisher publisher)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _publisher = publisher;
    }

    public async ValueTask<bool> Handle(StayCommand command, CancellationToken cT)
    {
        command.Request.Deconstruct(out Guid roomId, out Guid playerId);

        var roomResult = await _unitOfWork.RoomRepository.GetByIdAsync(roomId, cT);
        if (!roomResult.TryFromResult(out Room? room, out var roomErrors))
        {
            foreach (var error in roomErrors) _logger.LogError(error.Message);

            await _publisher.Publish(new UserNotificationEvent(
                    NotificationText: NotificationMessages.SomethingWentWrong(),
                    TargetId: playerId),
                cT);

            return false;
        }

        var playerStayResult = room!.PlayerStay(playerId);
        if (playerStayResult is DomainError playerStayError)
        {
            _logger.LogError(playerStayError.Reason);

            await _publisher.Publish(new UserNotificationEvent(
                    NotificationText: NotificationMessages.SomethingWentWrong(),
                    TargetId: playerId),
                cT);

            return false;
        }

        await _unitOfWork.SaveChangesAsync(cT);

        _logger.LogInformation("User {UserId}: Invoking command {CommandName} with argument {Argument}.",
            playerId,
            nameof(PassTurnCommand),
            new {RoomId = roomId});
        return await _mediator.Send(new PassTurnCommand(roomId), cT);
    }
}