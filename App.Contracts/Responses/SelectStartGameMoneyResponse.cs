namespace App.Contracts.Responses;

public sealed record SelectStartGameMoneyResponse(
    Guid RoomId,
    int LowerBound,
    int UpperBound,
    int AvailableUpperBound,
    int RecommendedValue);