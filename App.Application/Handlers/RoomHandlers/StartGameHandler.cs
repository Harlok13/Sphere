using App.Application.Extensions;
using App.Application.Messages;
using App.Application.Repositories.UnitOfWork;
using App.Application.Services.Interfaces;
using App.Domain.DomainResults;
using App.Domain.Entities.RoomEntity;
using App.Domain.Shared;
using App.SignalR.Commands.RoomCommands;
using App.SignalR.Events;
using Mediator;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.RoomHandlers;

public class StartGameHandler : ICommandHandler<StartGameCommand, bool>
{
    private readonly ILogger<StartGameHandler> _logger;
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly ICardsDeckService _cardsDeckService;
    private readonly IPublisher _publisher;

    public StartGameHandler(
        ILogger<StartGameHandler> logger,
        IAppUnitOfWork unitOfWork,
        ICardsDeckService cardsDeckService, 
        IPublisher publisher)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _cardsDeckService = cardsDeckService;
        _publisher = publisher;
    }
    
    public async ValueTask<bool> Handle(StartGameCommand command, CancellationToken cT)
    {
        command.Request.Deconstruct(out Guid roomId, out Guid playerId);

        var roomResult = await _unitOfWork.RoomRepository.GetByIdAsync(roomId, cT);
        if (!roomResult.TryFromResult(out Room? room, out var roomErrors))
        {
            return await SendSomethingWentWrongNotificationAsync(cT, playerId, errors: roomErrors);
        }

        var canStartGameResult = room!.CanStartGame(playerId);
        switch (canStartGameResult)
        {
            case DomainFailure canStartGameFailure:
                await _publisher.Publish(new UserNotificationEvent(
                        NotificationText: canStartGameFailure.Reason,
                        TargetId: playerId),
                    cT);

                return false;
            case DomainNotificationFailure notificationFailure:
                await _publisher.Publish(new UserNotificationEvent(
                        NotificationText: notificationFailure.Reason,
                        TargetId: playerId),
                    cT);

                await _publisher.Publish(new UsersNotificationEvent(
                        NotificationText: notificationFailure.NotificationForPlayers,
                        TargetIds: notificationFailure.PlayerIds),
                    cT);

                return false;
            case DomainError canStartGameError:
                return await SendSomethingWentWrongNotificationAsync(cT, playerId, singleError: canStartGameError);
        }

        var cardsDeck = _cardsDeckService.Create();
        var startGameResult = room.StartGame(leaderId: playerId, cardsDeck: cardsDeck);
        switch (startGameResult)
        {
            case DomainError startGameError:
                return await SendSomethingWentWrongNotificationAsync(cT, playerId, singleError: startGameError);
            case DomainFailure startGameFailure:
                await _publisher.Publish(new UserNotificationEvent(
                        NotificationText: startGameFailure.Reason,
                        TargetId: playerId),
                    cT);

                return false;
            default:
                return await _unitOfWork.SaveChangesAsync(cT);
        }
    }
    
    private async ValueTask<bool> SendSomethingWentWrongNotificationAsync(
        CancellationToken cT,
        Guid targetId,
        DomainError? singleError = default,
        IEnumerable<Error>? errors = default)
    {
        if (errors is not null)
            foreach (var error in errors) _logger.LogError(error.Message);
        
        if (singleError is not null) _logger.LogError(singleError.Reason);
            
        await _publisher.Publish(new UserNotificationEvent(
                NotificationText: NotificationMessages.SomethingWentWrong(),
                TargetId: targetId),
            cT);
            
        return false;
    }
}