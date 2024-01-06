using App.Application.Extensions;
using App.Application.Repositories.UnitOfWork;
using App.Contracts.Data;
using App.Contracts.Enums;
using App.Domain.Entities;
using App.SignalR.Commands.ConnectionCommands;
using App.SignalR.Events;
using Mediator;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.ConnectionHandlers;

public class ConnectPlayerHandler : ICommandHandler<ConnectPlayerCommand, bool>
{
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly ILogger<ConnectPlayerHandler> _logger;
    private readonly IPublisher _publisher;

    public ConnectPlayerHandler(
        IAppUnitOfWork unitOfWork,
        ILogger<ConnectPlayerHandler> logger,
        IPublisher publisher)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _publisher = publisher;
    }

    public async ValueTask<bool> Handle(ConnectPlayerCommand command, CancellationToken cT)
    {
        command.Deconstruct(out IUser authUser);

        if (!_unitOfWork.PlayerRepository.CheckPlayerExists(authUser.Id))
        {
            _logger.LogInformation(
                "The player \"{Name}\" is not in the room.",
                authUser.UserName);
            return false;
        }

        await _publisher.Publish(new UserNavigateEvent(
                TargetId: authUser.Id,
                Navigate: NavigateEnum.Lobby),
            cT);

        var roomIdResult = await _unitOfWork.RoomRepository.GetIdByPlayerIdAsync(authUser.Id, cT);
        if (!roomIdResult.TryFromResult(out RoomIdDto? data, out var errors))
        {
            foreach(var error in errors) _logger.LogError(error.Message);
            return false;
        }

        await _publisher.Publish(new UserReconnectToRoomEvent(
                RoomId: data!.RoomId,
                PlayerId: authUser.Id),
            cT);

        return true;
    }
}