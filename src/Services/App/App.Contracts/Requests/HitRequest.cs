namespace App.Contracts.Requests;

public sealed record HitRequest(
    Guid RoomId,
    Guid PlayerId);