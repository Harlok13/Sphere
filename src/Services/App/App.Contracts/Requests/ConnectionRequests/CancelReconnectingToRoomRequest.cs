namespace App.Contracts.Requests.ConnectionRequests;

public sealed record CancelReconnectingToRoomRequest(
    Guid RoomId,
    Guid PlayerId);