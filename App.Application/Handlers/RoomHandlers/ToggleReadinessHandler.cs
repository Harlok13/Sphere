using App.Application.Repositories;
using App.Application.Repositories.UnitOfWork;
using App.Contracts.Mapper;
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
        command.Request.Deconstruct(out Guid _, out Guid playerId);
        
        var player = await _unitOfWork.PlayerRepository.GetPlayerByIdAsync(playerId, cT);
        player.ToggleReadiness();

        await _unitOfWork.SaveChangesAsync(cT);

        return true;
    }
}