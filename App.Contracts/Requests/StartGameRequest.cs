namespace App.Contracts.Requests;

public record StartGameRequest(
    Guid RoomId,
    Guid PlayerId);