namespace App.Contracts.Requests.ConnectionRequests;

public sealed record ConfirmReconnectingToRoomRequest(
    Guid RoomId,
    Guid PlayerId);