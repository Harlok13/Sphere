namespace App.Contracts.Responses.PlayerResponses;

public sealed record ChangedPlayerInGameResponse(
    Guid PlayerId,
    bool InGame);