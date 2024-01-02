namespace App.Domain.Messages;

internal abstract partial class Message
{
    internal abstract class Failure
    { 
        internal abstract class Room
        {
            internal abstract class CanPlayerJoin
            {
                public static string RoomIsFull(string roomName)
                    => $"You can't join the room \"{roomName}\" because she is full.";

                public static string PlayerWasKicked(string roomName, string whoKickName)
                    => $"You can't join the room \"{roomName}\" because you were kicked by the player \"{whoKickName}\".";
            }

            internal abstract class JoinToRoom
            {
                public static string NotEnoughMoney(string roomName)
                    => $"You don't have enough money to join the room \"{roomName}\".";
            }
            
            internal abstract class KickPlayer
            {
                internal static string NotLeader()
                    => "You are not a leader to kick the player.";

                internal static string PlayerInGame(string kickedPlayerName)
                    => $"You can't kick the player \"{kickedPlayerName}\" while he is in the game.";
            }

            internal abstract class TransferLeadership
            {
                internal static string NotLeader(string receiverName)
                    => $"You can't transfer leadership to \"{receiverName}\" because you are not the leader.";
            }
        }
    }
}