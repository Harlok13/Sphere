namespace App.Domain.Entities;

public record CardInDeck(
    int X,
    int Y,
    int Width,
    int Height,
    int Value,
    string SuitValue);