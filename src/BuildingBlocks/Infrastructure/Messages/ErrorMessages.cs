namespace App.Infra.Messages;

internal abstract class ErrorMessages
{
    internal static string ArgumentIsNull(string argName, string methodName)
        => $"The argument \"{argName}\" of method \"{methodName}\" is null.";

    internal static string NotFound(string objName, string callingMethod)
        => $"Calling method \"{callingMethod}\": {objName} was not found.";

    internal abstract class Player
    {
        public static string IdIsNull() => "Player id is null.";

        public static string NotFound(string id) => $"Player with Id {id} was not found.";

        public static string ContainsMtOne(string id) => $"Source contains more than one player with ID {id}.";

        public static string SourceIsNull() => "Source is null";

        public static string OperationCanceled() => "The operation was canceled.";
    }

    internal abstract class Room
    {
        public static string IdIsNull()
            => "Room id is null.";

        public static string NotFound(Guid playerId)
            => $"Room with player ID \"{playerId}\" was not found.";
    }
}