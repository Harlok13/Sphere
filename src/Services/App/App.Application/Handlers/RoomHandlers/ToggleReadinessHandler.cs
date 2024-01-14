using App.Application.Extensions;
using App.Application.Messages;
using App.Application.Repositories.UnitOfWork;
using App.Domain.DomainResults;
using App.Domain.Entities.RoomEntity;
using App.SignalR.Commands.RoomCommands;
using App.SignalR.Events;
using Mediator;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.RoomHandlers;

public class ToggleReadinessHandler : ICommandHandler<ToggleReadinessCommand, bool>
{
    private readonly ILogger<ToggleReadinessHandler> _logger;
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher;

    public ToggleReadinessHandler(
        ILogger<ToggleReadinessHandler> logger,
        IAppUnitOfWork unitOfWork, 
        IPublisher publisher)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }
    
    public async ValueTask<bool> Handle(ToggleReadinessCommand command, CancellationToken cT)
    {
        command.Request.Deconstruct(out Guid roomId, out Guid playerId);

        var roomResult = await _unitOfWork.RoomRepository.GetByIdAsync(roomId, cT);
        if (!roomResult.TryFromResult(out Room? room, out var roomErrors))
        {
            foreach(var error in roomErrors) _logger.LogError(error.Message);

            await _publisher.Publish(new UserNotificationEvent(
                    NotificationText: NotificationMessages.SomethingWentWrong(),
                    TargetId: playerId),
                cT);

            return false;
        }

        var playerToggleReadinessResult = room!.PlayerToggleReadiness(playerId);
        if (playerToggleReadinessResult is DomainError playerToggleReadinessError)
        {
            _logger.LogError(playerToggleReadinessError.Reason);

            await _publisher.Publish(new UserNotificationEvent(
                    NotificationText: NotificationMessages.SomethingWentWrong(),
                    TargetId: playerId),
                cT);

            return false;
        }
        
        return await _unitOfWork.SaveChangesAsync(cT);
    }
}