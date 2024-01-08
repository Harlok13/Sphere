namespace App.Contracts.Requests;

public sealed record StopTimerRequest(
    Guid RoomId,
    Guid PlayerId);