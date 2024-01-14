namespace App.Contracts.Requests;

public sealed record StayRequest(
    Guid RoomId,
    Guid PlayerId);