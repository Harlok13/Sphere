namespace App.Contracts.Responses.PlayerResponses;

public sealed record ChangedPlayerMoneyResponse(
    Guid PlayerId,
    int Money);