namespace App.Domain.Entities;

public struct Card
{
    private Card(int x, int y, int width, int height, int value, string suitValue)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        Value = value;
        SuitValue = suitValue;
    }
    
    public int X { get; private init; }
    public int Y { get; private init; }
    public int Width { get; private init; }
    public int Height { get; private init; }
    public int Value { get; private init; }
    public string SuitValue { get; private init; }

    // public Guid PlayerId { get; set;} 

    public static Card Create(int x, int y, int width, int height, int value, string suitValue) =>
        new Card(x, y, width, height, value, suitValue);

}