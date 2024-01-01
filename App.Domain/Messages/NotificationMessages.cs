namespace App.Domain.Messages;

internal abstract class NotificationMessages
{
    internal abstract class Room
    {
        internal abstract class KickPlayer
        {
            internal static string NotLeader() 
                => "You are not a leader to kick the player.";

            internal static string PlayerInGame(string kickedPlayerName) 
                => $"You can't kick the player \"{kickedPlayerName}\" while he is in the game.";
        }

        internal abstract class AddKickedPlayer
        {
            public static string WasKicked(string kickInitiatorName)
                => $"You were kicked out of the room by \"{kickInitiatorName}\". You can join if a member of this room invites you.";

            public static string SuccessKick(string kickedPlayerName)
                => $"You kick \"{kickedPlayerName}\" from room.";
        }
    }
}