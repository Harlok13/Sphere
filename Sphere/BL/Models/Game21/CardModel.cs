using Sphere.BL.Game21.Enums;

namespace Sphere.BL.Models.Game21;

public class CardModel
{
    public int X { get; }
    public int Y { get; }
    public int Width { get; }
    public int Height { get; }
    public int Value { get; }
    public string SuitValue { get; }

    public CardOwnerEnum Owner { get; set;} = CardOwnerEnum.Undefined;

    public CardModel(int x, int y, int width, int height, int value, string suitValue)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        Value = value;
        SuitValue = suitValue;
    }
}