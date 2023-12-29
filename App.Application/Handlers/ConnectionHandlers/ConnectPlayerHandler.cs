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

        var playerResult = await _unitOfWork.PlayerRepository.GetPlayerByIdAsNoTrackingAsync(authUser.Id, cT);
        if (!playerResult.TryFromResult(out PlayerDto? playerDto, out _))
        {
            _logger.LogInformation("The player {Name} is not in the room.", authUser.UserName);
            return false;
        }

        await _hubContext.Clients.User(playerDto!.Id.ToString()).ReceiveUser_NavigateToLobby(cT);
        _logger.LogInformation("");

        var response = new ReconnectToRoomResponse(playerDto.RoomId);

        await _hubContext.Clients.User(playerDto.Id.ToString()).ReceiveUser_ReconnectToRoom(response, cT);
        _logger.LogInformation("");

        return true;
    }
}