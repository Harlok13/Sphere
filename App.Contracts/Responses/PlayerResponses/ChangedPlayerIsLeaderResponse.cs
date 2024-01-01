namespace App.Contracts.Responses.PlayerResponses;

public sealed record ChangedPlayerIsLeaderResponse(
    Guid PlayerId,
    bool IsLeader);