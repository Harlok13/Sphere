namespace App.Domain.Messages;

internal abstract partial class Message
{
    internal abstract class Failure
    { 
        internal abstract class Room
        {
            internal abstract class CanPlayerJoin
            {
                internal static string RoomIsFull(string roomName)
                    => $"You can't join the room \"{roomName}\" because she is full.";

                internal static string PlayerWasKicked(string roomName, string whoKickName)
                    => $"You can't join the room \"{roomName}\" because you were kicked by the player \"{whoKickName}\".";
            }

            internal abstract class JoinToRoom
            {
                internal static string NotEnoughMoney(string roomName)
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

            internal abstract class CanStartGame
            {
                internal static string NotEnoughPlayers()
                    => "You cannot start the game. Minimum number of participants - 2.";

                internal static string NotLeader()
                    => "Only the leader can start the game.";

                internal static string NotReadiness()
                    => "You didn't press the \"ready\" button.";

                internal static string PlayersNotReady(string playersList)
                    => $"Not all players ({playersList}) are ready.";

                internal static string SomeoneNotEnoughMoney(string playersList)
                    => $"Not all players ({playersList}) have enough money for the start bet.";
            }

            internal abstract class GetNextCard
            {
                internal static string DeckIsOut()
                    => "The deck is out.";
            }

            internal abstract class RemovePlayerFromRoom
            {
                internal static string InGame()
                    => "You can't leave the room during the game.";
            }

            internal abstract class PlayerHit
            {
                internal static string MaxCards(int maxCardsCount)
                    => $"You can't draw more cards. Maximum number of cards {maxCardsCount}.";
            }
        }

        internal abstract class Player
        {
            internal abstract class DecreaseMoney
            {
                internal static string NotEnoughMoney(int moneyDiff)
                    => $"You don't have enough money - {moneyDiff}$.";
            }
        }
    }
}