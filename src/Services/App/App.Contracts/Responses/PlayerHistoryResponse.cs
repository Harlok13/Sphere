using App.Domain.Enums;

namespace App.Contracts.Responses;

public sealed record PlayerHistoryResponse(
    Guid Id,
    string Score,
    DateTime PlayedAt,
    string CardsPlayed,
    GameResult Result);