using App.Application.Extensions;
using App.Application.Repositories.UnitOfWork;
using App.Application.Services.Interfaces;
using App.Domain.DomainResults;
using App.Domain.Entities;
using App.Domain.Entities.RoomEntity;
using App.SignalR.Commands.RoomCommands;
using App.SignalR.Commands.RoomCommands.GameActionCommands;
using App.SignalR.Events;
using Mediator;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.RoomHandlers.GameActionHandlers;

public class HitHandler : ICommandHandler<HitCommand, bool>
{
    private readonly ILogger<HitHandler> _logger;
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IPublisher _publisher;
    private readonly ICardsDeckService _cardsDeck;

    public HitHandler(
        ILogger<HitHandler> logger,
        IAppUnitOfWork unitOfWork,
        IMediator mediator,
        ICardsDeckService cardsDeck, 
        IPublisher publisher)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _cardsDeck = cardsDeck;
        _publisher = publisher;
    }

    public async ValueTask<bool> Handle(HitCommand command, CancellationToken cT)
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

        var card = await _cardsDeck.GetNextCardAsync(roomId: roomId, playerId: playerId, cT); 
        
        var playerHitResult = room!.PlayerHit(playerId: playerId, card: card);
        if (playerHitResult is DomainError playerHitError)
        {
            _logger.LogError(playerHitError.Reason);

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