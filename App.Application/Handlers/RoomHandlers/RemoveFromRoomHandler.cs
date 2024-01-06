using App.Application.Extensions;
using App.Application.Repositories.UnitOfWork;
using App.Contracts.Enums;
using App.Domain.DomainResults;
using App.Domain.Entities.PlayerInfoEntity;
using App.Domain.Entities.RoomEntity;
using App.Domain.Extensions;
using App.SignalR.Commands.RoomCommands;
using App.SignalR.Events;
using Mediator;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.RoomHandlers;

public class RemoveFromRoomHandler : ICommandHandler<RemoveFromRoomCommand, bool>
{
    private readonly ILogger<RemoveFromRoomHandler> _logger;
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher;
    
    public RemoveFromRoomHandler(
        ILogger<RemoveFromRoomHandler> logger,
        IAppUnitOfWork unitOfWork,
        IPublisher publisher)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    public async ValueTask<bool> Handle(RemoveFromRoomCommand command, CancellationToken cT)
    {
        command.Request.Deconstruct(out Guid roomId, out Guid playerId);

        var roomResult = await _unitOfWork.RoomRepository.GetByIdAsync(roomId, cT);
        if (!roomResult.TryFromResult(out Room? room, out var roomErrors))
        {
            foreach(var error in roomErrors) _logger.LogError(error.Message);

            return false;
        }
        
        var removePlayerResult = room!.RemovePlayerFromRoom(playerId);
        if (removePlayerResult is DomainError removePlayerError)
        {
            _logger.LogError(removePlayerError.Reason);
            
            return false;
        }

        if (removePlayerResult is DomainFailure removePlayerFailure)
        {
            await _publisher.Publish(new UserNotificationEvent(
                    NotificationText: removePlayerFailure.Reason,
                    TargetId: playerId),
                cT);
            
            return false;
        }

        if (!removePlayerResult.TryFromDomainResult(out int incrementMoney, out DomainError? removeError))
        {
            _logger.LogError(removeError!.Reason);

            return false;
        }

        var playerInfoResult = await _unitOfWork.PlayerInfoRepository.GetPlayerInfoByIdAsync(playerId, cT);
        if (!playerInfoResult.TryFromResult(out PlayerInfo? playerInfo, out var playerInfoErrors))
        {
            foreach (var error in playerInfoErrors) _logger.LogError(error.Message);

            return false;
        }
        playerInfo!.IncrementMoney(incrementMoney);
        
        var saveChangesResult = await _unitOfWork.SaveChangesAsync(cT);
        if (saveChangesResult)
        {
            await _publisher.Publish(new UserNavigateEvent(
                    TargetId: playerId,
                    Navigate: NavigateEnum.Lobby),
                cT);
        }
        
        return saveChangesResult;
    }
}
