namespace App.Contracts.Responses.PlayerResponses;

public sealed record ChangedPlayerOnlineResponse(
    bool Online,
    Guid PlayerId);