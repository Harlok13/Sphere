using App.Application.Extensions;
using App.Application.Repositories.UnitOfWork;
using App.Domain.DomainResults;
using App.Domain.DomainResults.CustomResults;
using App.Domain.Entities.PlayerInfoEntity;
using App.Domain.Entities.RoomEntity;
using App.SignalR.Commands.RoomCommands;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.RoomHandlers;

public class RemoveFromRoomHandler : ICommandHandler<RemoveFromRoomCommand, bool>
{
    private readonly ILogger<RemoveFromRoomHandler> _logger;
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;

    public RemoveFromRoomHandler(
        ILogger<RemoveFromRoomHandler> logger,
        IAppUnitOfWork unitOfWork,
        IHubContext<GlobalHub, IGlobalHub> hubContext)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _hubContext = hubContext;
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

        var removePlayerData = removePlayerResult as RemovePlayerFromRoomDomainResult;

        
        var playerInfoResult = await _unitOfWork.PlayerInfoRepository.GetPlayerInfoByIdAsync(playerId, cT);
        if (!playerInfoResult.TryFromResult(out PlayerInfo? playerInfo, out var playerInfoErrors))
        {
            foreach (var error in playerInfoErrors) _logger.LogError(error.Message);

            return false;
        }
        playerInfo!.IncrementMoney(removePlayerData!.IncrementMoney);
        
        var saveChangesResult = await _unitOfWork.SaveChangesAsync(cT);
        if (saveChangesResult)
        {
            await _hubContext.Clients.User(playerId.ToString()).ReceiveUser_NavigateToLobby(cT);
        }
        
        return saveChangesResult;
    }
}
