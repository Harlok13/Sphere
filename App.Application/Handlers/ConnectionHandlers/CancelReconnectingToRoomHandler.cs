using App.Application.Extensions;
using App.Application.Messages;
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
            _logger.LogError(
                "Unable to perform \"{CommandName}\" operation because connection id is null.",
                nameof(DisconnectPlayerCommand));

            return false;
        }

        var roomResult = await _unitOfWork.RoomRepository.GetIdByPlayerIdAsync(authUser.Id, cT);
        if (!roomResult.TryFromResult(out RoomIdDto? data, out var errors))
        {
            foreach (var error in errors) _logger.LogError(error.Message);

            await _publisher.Publish(new UserNotificationEvent(
                    NotificationText: NotificationMessages.SomethingWentWrong(),
                    TargetId: authUser.Id),
                cT);

            return false;
        }
        
        var request = new RemoveFromRoomRequest(data!.RoomId, authUser.Id);
        _logger.LogInformation("User {UserId}: Invoking command {CommandName} with argument {Argument}.",
            authUser.Id,
            nameof(RemoveFromRoomCommand),
            new {Request = request});
        return await _mediator.Send(new RemoveFromRoomCommand(request), cT);
    }
}