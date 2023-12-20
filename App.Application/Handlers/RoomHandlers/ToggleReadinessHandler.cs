using App.Application.Mapper;
using App.Application.Repositories;
using App.Application.Repositories.UnitOfWork;
using App.SignalR.Commands.RoomCommands;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Player = App.Domain.Entities.PlayerEntity.Player;

namespace App.Application.Handlers.RoomHandlers;

public class ToggleReadinessHandler : ICommandHandler<ToggleReadinessCommand, bool>
{
    private readonly ILogger<ToggleReadinessHandler> _logger;
    private readonly IPlayerRepository _playerRepository;
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;

    public ToggleReadinessHandler(
        ILogger<ToggleReadinessHandler> logger,
        IPlayerRepository playerRepository,
        IAppUnitOfWork unitOfWork,
        IHubContext<GlobalHub, IGlobalHub> hubContext)
    {
        _logger = logger;
        _playerRepository = playerRepository;
        _unitOfWork = unitOfWork;
        _hubContext = hubContext;
    }
    
    public async ValueTask<bool> Handle(ToggleReadinessCommand command, CancellationToken cT)
    {
        command.Deconstruct(out Guid roomId, out Guid playerId, out string connectionId);
        
        var player = await _playerRepository.GetPlayerByIdAsync(playerId, cT);
        player.ToggleReadiness();

        var playerResponse = PlayerMapper.MapPlayerToPlayerResponse(player);
        await _hubContext.Clients.Group(roomId.ToString()).ReceiveGroup_UpdatedPlayer(playerResponse, cT);  // TODO: receive player id only
        // await _hubContext.Clients.Group(roomId.ToString()).ReceiveGroup_UpdatedReadiness(playerId, cT);
        await _hubContext.Clients.Client(connectionId).ReceiveOwn_Readiness(player.Readiness, cT);

        await _unitOfWork.SaveChangesAsync(cT);

        return true;
    }

    private async Task NotifyReadinessAsync(Player player, CancellationToken cT)
    {
        await _hubContext.Clients.Client(player.ConnectionId).ReceiveOwn_Readiness(player.Readiness, cT);
    }
}