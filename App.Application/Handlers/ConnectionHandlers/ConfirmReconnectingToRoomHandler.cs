using App.Application.Extensions;
using App.Application.Repositories.UnitOfWork;
using App.Contracts.Data;
using App.Contracts.Mapper;
using App.Contracts.Responses;
using App.Domain.Entities;
using App.Domain.Entities.PlayerEntity;
using App.Domain.Entities.RoomEntity;
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
            return await SendSomethingWentWrongNotification(roomErrors, authUser.ConnectionId, cT);
        }

        var reconnectPlayerResult = room!.ReconnectPlayer(playerId: authUser.Id, newConnectionId: authUser.ConnectionId);
        if (!reconnectPlayerResult.TryFromResult(out Player? reconnectPlayer, out var reconnectErrors))
        {
            return await SendSomethingWentWrongNotification(reconnectErrors, authUser.ConnectionId, cT);
        }
        
        var saveChangesResult = await _unitOfWork.SaveChangesAsync(cT);
        if (!saveChangesResult)
        {
            var errorMsg = new Error($"Transaction failed when calling {nameof(ConfirmReconnectingToRoomCommand)}");
            return await SendSomethingWentWrongNotification(new List<Error>(1) { errorMsg }, authUser.ConnectionId, cT);
        }

        var playerDto = PlayerMapper.MapPlayerToPlayerDto(reconnectPlayer!);  // TODO: RoomMapper.MapToReconnectingInitRoomDataResponse
        var initRoomDataDto = RoomMapper.MapRoomToInitRoomDataDto(room);
        var playersDto = PlayerMapper.MapManyPlayersToManyPlayersDto(room.Players);

        var response = new ReconnectingInitRoomDataResponse(
            Player: playerDto,
            InitRoomData: initRoomDataDto,
            Players: playersDto);

        await _hubContext.Clients.Client(reconnectPlayer!.ConnectionId).ReceiveClient_ReconnectingInitRoomData(response, cT);
        _logger.LogInformation("");

        return saveChangesResult;
    }

    private async ValueTask<bool> SendSomethingWentWrongNotification(
        IEnumerable<Error> errors,
        string targetConnectionId,
        CancellationToken cT)
    {
        foreach (var error in errors) _logger.LogError(error.Message);
            
        await _publisher.Publish(new ClientNotificationEvent(
                NotificationText: NotificationMessages.SomethingWentWrong(),
                TargetConnectionId: targetConnectionId),
            cT);
            
        return false;
    }
}