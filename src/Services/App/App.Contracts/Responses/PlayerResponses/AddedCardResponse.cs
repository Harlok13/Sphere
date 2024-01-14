using App.Contracts.Data;

namespace App.Contracts.Responses.PlayerResponses;

public sealed record AddedCardResponse(
    Guid PlayerId,
    CardDto CardDto);