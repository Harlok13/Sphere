namespace App.Contracts.Responses;

public sealed record SelectStartGameMoneyResponse(
    int LowerBound,
    int UpperBound,
    int AvailableUpperBound,
    int RecommendedValue,
    Guid? RoomId);