namespace App.Contracts.Data;

public record CardDto(
    Guid Id,
    Guid PlayerId,
    int X,
    int Y,
    int Width,
    int Height,
    int Value,
    string SuitValue);