namespace App.Domain.Messages;

internal abstract partial class Message
{
    internal abstract class Notification
    {
        internal abstract class Room
        {
            internal abstract class AddKickedPlayer
            {
                public static string WasKicked(string kickInitiatorName)
                    => $"You were kicked out of the room by \"{kickInitiatorName}\". You can join if a member of this room invites you.";

                public static string SuccessKick(string kickedPlayerName)
                    => $"You kick \"{kickedPlayerName}\" from room.";
            }

            internal abstract class TransferLeadership
            {
                public static string SuccessTransfer(string receiverName)
                    => $"You have given leadership to the player \"{receiverName}\".";

                public static string ReceiveLeadership(string senderName)
                    => $"The player \"{senderName}\" gave you the lead.";
            }
        }
    }
}