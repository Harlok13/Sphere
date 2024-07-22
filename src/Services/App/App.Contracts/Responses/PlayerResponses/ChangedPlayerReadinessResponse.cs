namespace App.Contracts.Responses.PlayerResponses;

public sealed record ChangedPlayerReadinessResponse(
    Guid PlayerId,
    bool Readiness);