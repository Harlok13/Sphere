using App.Application.Extensions;
using App.Application.Messages;
using App.Application.Repositories.UnitOfWork;
using App.Contracts.Enums;
using App.Contracts.Mapper;
using App.Contracts.Responses;
using App.Domain.DomainResults;
using App.Domain.DomainResults.CustomResults;
using App.Domain.Entities;
using App.Domain.Entities.PlayerEntity;
using App.Domain.Entities.RoomEntity;
using App.Domain.Extensions;
using App.Domain.Shared;
using App.SignalR.Commands.ConnectionCommands;
using App.SignalR.Events;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.ConnectionHandlers;

public class ConfirmReconnectingToRoomHandler : ICommandHandler<ConfirmReconnectingToRoomCommand, bool>
{
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly ILogger<ConfirmReconnectingToRoomHandler> _logger;
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly IPublisher _publisher;

    public ConfirmReconnectingToRoomHandler(
        IAppUnitOfWork unitOfWork,
        ILogger<ConfirmReconnectingToRoomHandler> logger,
        IHubContext<GlobalHub, IGlobalHub> hubContext,
        IPublisher publisher)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _hubContext = hubContext;
        _publisher = publisher;
    }

    public async ValueTask<bool> Handle(ConfirmReconnectingToRoomCommand command, CancellationToken cT)
    {
        command.Deconstruct(out AuthUser authUser);
        if (authUser.ConnectionId is null)
        {
            _logger.LogError(
                "Unable to perform \"{CommandName}\" operation because connection id is null.",
                nameof(DisconnectPlayerCommand));

            return false;
        }
        
        var roomResult = await _unitOfWork.RoomRepository.GetByPlayerIdAsync1(authUser.Id, cT);
        if (!roomResult.TryFromResult(out Room? room, out var roomErrors))
        {
            return await SendSomethingWentWrongNotificationAsync(cT, authUser.ConnectionId, errors: roomErrors);
        }

        var reconnectPlayerResult = room!.ReconnectPlayer(playerId: authUser.Id, newConnectionId: authUser.ConnectionId);
        if (reconnectPlayerResult is DomainError reconnectPlayerError)
        {
            return await SendSomethingWentWrongNotificationAsync(cT, authUser.ConnectionId, singleError: reconnectPlayerError);
        }

        if (!reconnectPlayerResult.TryFromDomainResult(out Player? reconnectPlayer, out DomainError? error))
        {
            return await SendSomethingWentWrongNotificationAsync(cT, authUser.ConnectionId, singleError: error);
        }

        var saveChangesResult = await _unitOfWork.SaveChangesAsync(cT);
        if (!saveChangesResult)
        {
            _logger.LogError($"Transaction failed when calling {nameof(ConfirmReconnectingToRoomCommand)}");
            return await SendSomethingWentWrongNotificationAsync(cT, authUser.ConnectionId);
        }

        var playerDto = PlayerMapper.MapPlayerToPlayerDto(reconnectPlayer!);  // TODO: RoomMapper.MapToReconnectingInitRoomDataResponse
        var initRoomDataDto = RoomMapper.MapRoomToInitRoomDataDto(room);
        var playersDto = PlayerMapper.MapManyPlayersToManyPlayersDto(room.Players);

        var response = new ReconnectingInitRoomDataResponse(  // TODO: init cards
            Player: playerDto,
            InitRoomData: initRoomDataDto,
            Players: playersDto);

        await _hubContext.Clients  // TODO: event handler
            .Client(reconnectPlayer!.ConnectionId)
            .ReceiveClient_ReconnectingInitRoomData(response, cT);
        _logger.LogInformation("");

        await _publisher.Publish(new UserNavigateEvent(
                TargetId: authUser.Id,
                NavigateEnum.Room),
            cT);

        return saveChangesResult;
    }

    private async ValueTask<bool> SendSomethingWentWrongNotificationAsync(
        CancellationToken cT,
        string targetConnectionId,
        DomainError? singleError = default,
        IEnumerable<Error>? errors = default)
    {
        if (errors is not null)
            foreach (var error in errors) _logger.LogError(error.Message);
        
        if (singleError is not null)
            _logger.LogError(singleError.Reason);
            
        await _publisher.Publish(new ClientNotificationEvent(
                NotificationText: NotificationMessages.SomethingWentWrong(),
                TargetConnectionId: targetConnectionId),
            cT);
            
        return false;
    }
}