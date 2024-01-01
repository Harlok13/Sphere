namespace App.Domain.Messages;

internal abstract class ErrorMessages
{
    internal abstract class Room
    {
        internal abstract class TransferLeadership
        {
            public static string SenderNotFound()
                => "Leadership could not be transferred, so the sender was not found.";

            public static string ReceiverNotFound()
                => "Leadership could not be transferred, so the receiver was not found.";
        }

        internal abstract class RemoveFromRoom
        {
            public static string PlayerNotFound(Guid removedPlayerId)
                => $"The player to be removed with ID {removedPlayerId} was not found.";
        }

        internal abstract class SetNewRoomLeader
        {
            public static string PlayerNotFound()
                => "A new leader was not found.";
        }

        internal abstract class ReconnectPlayer
        {
            public static string PlayerNotFound(Guid reconnectPlayerId)
                => $"The reconnected player with ID {reconnectPlayerId} was not found.";
        }

        internal abstract class DisconnectPlayer
        {
            public static string PlayerNotFound(Guid disconnectPlayerId)
                => $"The disconnected player with ID {disconnectPlayerId} was not found.";
        }

        internal abstract class CanPlayerJoin
        {
            public static string RoomIsFull(string roomName)
                => $"You can't join the room \"{roomName}\" because she is full.";

            public static string PlayerWasKicked(string roomName, string whoKickName)
                => $"You can't join the room \"{roomName}\" because you were kicked by the player \"{whoKickName}\".";
        }
    }
}