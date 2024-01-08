using App.Application.Repositories.UnitOfWork;
using App.SignalR.Commands.RoomCommands;
using Mediator;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.RoomHandlers;

public class RemoveRoomHandler : ICommandHandler<RemoveRoomCommand, bool>
{
    private readonly ILogger<RemoveRoomHandler> _logger;
    private readonly IAppUnitOfWork _unitOfWork;

    public RemoveRoomHandler(ILogger<RemoveRoomHandler> logger, IAppUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async ValueTask<bool> Handle(RemoveRoomCommand command, CancellationToken cT)
    {
        command.Deconstruct(out Guid roomId);

        _logger.LogInformation("Trying to remove the room with ID \"{RoomId}\".", roomId);
        var removeResult = await _unitOfWork.RoomRepository.RemoveAsync(roomId, cT);
        if (removeResult.IsFailure)
        {
            foreach (var error in removeResult.Errors!)
            {
                _logger.LogError(error.Message);
            }

            return false;
        }

        return await _unitOfWork.SaveChangesAsync(cT);
    }
}