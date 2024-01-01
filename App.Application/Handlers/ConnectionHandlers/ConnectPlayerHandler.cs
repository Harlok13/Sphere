using App.Application.Extensions;
using App.Application.Repositories.UnitOfWork;
using App.Contracts.Data;
using App.Contracts.Responses;
using App.Domain.Entities;
using App.SignalR.Commands.ConnectionCommands;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.ConnectionHandlers;

public class ConnectPlayerHandler : ICommandHandler<ConnectPlayerCommand, bool>
{
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly ILogger<ConnectPlayerHandler> _logger;
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;

    public ConnectPlayerHandler(IAppUnitOfWork unitOfWork, ILogger<ConnectPlayerHandler> logger, IHubContext<GlobalHub, IGlobalHub> hubContext)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _hubContext = hubContext;
    }

    public async ValueTask<bool> Handle(ConnectPlayerCommand command, CancellationToken cT)
    {
        command.Deconstruct(out IUser authUser);

        if (!_unitOfWork.PlayerRepository.CheckPlayerExists(authUser.Id))
        {
            _logger.LogInformation("The player {Name} is not in the room.", authUser.UserName);
            return false;
        }

        await _hubContext.Clients.User(authUser.Id.ToString()).ReceiveUser_NavigateToLobby(cT);
        _logger.LogInformation("");

        var roomIdResult = await _unitOfWork.RoomRepository.GetIdByPlayerIdAsync(authUser.Id, cT);
        if (!roomIdResult.TryFromResult(out RoomIdDto? data, out var errors))
        {
            foreach(var error in errors) _logger.LogError(error.Message);
            return false;
        }

        var response = new ReconnectToRoomResponse(data!.RoomId);

        await _hubContext.Clients.User(authUser.Id.ToString()).ReceiveUser_ReconnectToRoom(response, cT);
        _logger.LogInformation("");

        return true;
    }
}