namespace App.Contracts.Responses.RoomResponses;

public sealed record ChangedRoomAvatarUrlResponse(
    Guid RoomId,
    string AvatarUrl);