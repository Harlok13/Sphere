namespace App.Contracts.Responses;

public record CardResponse(
    Guid Id,
    Guid PlayerId,
    int X,
    int Y,
    int Width,
    int Height,
    int Value,
    string SuitValue);