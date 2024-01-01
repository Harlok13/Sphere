namespace App.Application;

public abstract class NotificationMessages
{
    public static string SomethingWentWrong()
        => "Something went wrong, please try again later.";
    
    public abstract class KickPlayerFromRoom
    {
        public static string NotLeader() 
            => "You are not a leader to kick the player.";

        public static string PlayerInGame(string kickedPlayerName) 
            => $"You can't kick the player \"{kickedPlayerName}\" while he is in the game.";
        
        public static string SuccessKick(string kickedPlayerName)
            => $"You kick \"{kickedPlayerName}\" from room.";

        public static string WasKicked(string kickInitiatorName)
            => $"You were kicked out of the room by \"{kickInitiatorName}\".";

        public static string JoinToRoom_WasKicked()
            => "You were kicked out of this room. You can join if a member of this room invites you.";
    }

    public abstract class TransferLeadership
    {
        public static string NotLeader()
            => "You are not a leader to pass on leadership.";

        public static string SuccessTransfer(string receiverName)
            => $"You have given leadership to the player \"{receiverName}\".";

        public static string ReceiveLeadership(string senderName)
            => $"The player \"{senderName}\" gave you the lead.";
    }
}