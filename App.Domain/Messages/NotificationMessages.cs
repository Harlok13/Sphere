namespace App.Domain.Messages;

internal abstract partial class Message
{
    internal abstract class Notification
    {
        internal abstract class Room
        {
            internal abstract class AddKickedPlayer
            {
                internal static string WasKicked(string kickInitiatorName)
                    => $"You were kicked out of the room by \"{kickInitiatorName}\". You can join if a member of this room invites you.";

                internal static string SuccessKick(string kickedPlayerName)
                    => $"You kick \"{kickedPlayerName}\" from room.";
            }

            internal abstract class TransferLeadership
            {
                internal static string SuccessTransfer(string receiverName)
                    => $"You have given leadership to the player \"{receiverName}\".";

                internal static string ReceiveLeadership(string senderName)
                    => $"The player \"{senderName}\" gave you the lead.";
            }

            internal abstract class CanStartGame
            {
                internal static string NotReady()
                    => "The leader wants to start the game but you are not prepared.";

                internal static string NotEnoughMoney()
                    => "The leader wants to start the game but you don't have enough money for the start bet.";
            }
        }
    }
}