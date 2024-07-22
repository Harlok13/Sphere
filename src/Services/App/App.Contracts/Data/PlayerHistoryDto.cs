using App.Domain.Enums;

namespace App.Contracts.Data;

public sealed record PlayerHistoryDto(
    Guid Id,
    string Score,
    DateTime PlayedAt,
    string CardsPlayed,
    GameResult Result);