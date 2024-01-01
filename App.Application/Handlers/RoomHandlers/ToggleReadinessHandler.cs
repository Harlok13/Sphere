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
    private readonly IAppUnitOfWork _unitOfWork;

    public ToggleReadinessHandler(
        ILogger<ToggleReadinessHandler> logger,
        IAppUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public async ValueTask<bool> Handle(ToggleReadinessCommand command, CancellationToken cT)
    {
        command.Request.Deconstruct(out Guid roomId, out Guid playerId);

        var room = await _unitOfWork.RoomRepository.GetByIdAsync(roomId, cT);
        var player = room.Players.Single(p => p.Id == playerId);
        player.ToggleReadiness();

        return await _unitOfWork.SaveChangesAsync(cT);
    }
}