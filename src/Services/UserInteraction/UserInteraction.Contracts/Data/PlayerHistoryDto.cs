using UserInteraction.Domain.Enums;

namespace UserInteraction.Contracts.Data;

public sealed record PlayerHistoryDto(
    Guid Id,
    string Score,
    DateTime PlayedAt,
    string CardsPlayed,
    EGameResult Result);