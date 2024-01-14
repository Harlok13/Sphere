namespace App.Contracts.Requests;

public sealed record TransferLeadershipRequest(
    Guid SenderId,
    Guid ReceiverId,
    Guid RoomId);