using App.Application.Extensions;
using App.Application.Repositories.UnitOfWork;
using App.Domain.Entities.RoomEntity;
using App.Domain.Enums;
using App.SignalR.Commands.RoomCommands;
using Mediator;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.RoomHandlers;

public class EndCycleHandler : ICommandHandler<EndCycleCommand, bool>
{
    private readonly ILogger<EndCycleHandler> _logger;
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public EndCycleHandler(
        ILogger<EndCycleHandler> logger,
        IAppUnitOfWork unitOfWork,
        IMediator mediator
    )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }
    
    public async ValueTask<bool> Handle(EndCycleCommand command, CancellationToken cT)
    {
        command.Deconstruct(out Guid roomId);

        var roomResult = await _unitOfWork.RoomRepository.GetByIdAsync(roomId, cT);
        if (!roomResult.TryFromResult(out Room? room, out var roomErrors))
        {
            foreach(var error in roomErrors) _logger.LogError(error.Message);

            return false;
        }
        
        var players = room!.Players;

        if (players.All(p => p.MoveStatus == MoveStatus.Stay))
        {
            _logger.LogInformation("Invoking command {CommandName} with argument {Argument}.",
                nameof(EndGameCommand),
                new {RoomId = roomId});
            return await _mediator.Send(new EndGameCommand(roomId), cT);
        }

        var cPlayers = players.Where(p => p.MoveStatus != MoveStatus.Stay);
        foreach (var player in cPlayers)
        {
            player.SetMoveStatus(MoveStatus.None);
        }

        // TODO: check cards count and MoveStatus
        await _unitOfWork.SaveChangesAsync(cT);
        
        _logger.LogInformation("Invoking command {CommandName} with argument {Argument}.",
            nameof(PassTurnCommand),
            new {RoomId = roomId});
        return await _mediator.Send(new PassTurnCommand(roomId), cT);
    }
}