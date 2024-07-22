namespace App.Domain.Messages;

internal abstract partial class Message
{
    internal abstract class Error
    {
        internal static string DeserializeError(string fieldName, string className)
            => $"Deserialization of \"{fieldName}\" field in \"{className}\" class failed.";
        
        internal static string FieldIsNull(string fieldName, string className)
            => $"The \"{fieldName}\" field in \"{className}\" class is null.";
        
        internal abstract class Room
        {
            internal static string PlayerNotFound(string invokedMethod, Guid? playerId)
                => $"{invokedMethod}: The player with ID \"{playerId}\" was not found.";
        }
    }
}