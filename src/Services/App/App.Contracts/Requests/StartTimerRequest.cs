namespace App.Contracts.Requests;

public sealed record StartTimerRequest(
    Guid RoomId,
    Guid PlayerId);