namespace App.Contracts.Responses.PlayerResponses;

public sealed record UpdatedPlayerIsLeaderResponse(
    Guid PlayerId,
    bool IsLeader);