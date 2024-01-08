namespace App.Contracts.Responses.RoomResponses;

public sealed record ChangedRoomRoomNameResponse(
    Guid RoomId,
    string RoomName);