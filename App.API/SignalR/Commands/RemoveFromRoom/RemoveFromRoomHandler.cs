using Mediator;

namespace Sphere.SignalR.Commands.RemoveFromRoom;

public class RemoveFromRoomHandler : ICommandHandler<RemoveFromRoomCommand, bool>
{
    public async ValueTask<bool> Handle(RemoveFromRoomCommand command, CancellationToken cancellationToken)
    {
        command.Deconstruct(out Guid roomId, out Guid playerId);

        return true;
    }
}