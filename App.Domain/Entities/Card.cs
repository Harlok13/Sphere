using System.ComponentModel.DataAnnotations.Schema;
using App.Domain.Primitives;

namespace App.Domain.Entities;

public class Card
{
    private Card(Guid id, int x, int y, int width, int height, int value, string suitValue)
    {
        Id = id;
        X = x;
        Y = y;
        Width = width;
        Height = height;
        Value = value;
        SuitValue = suitValue;
    }

    private Card(Guid id, Guid playerId, CardInDeck cardInDeck)
    {
        Id = id;
        PlayerId = playerId;
        X = cardInDeck.X;
        Y = cardInDeck.Y;
        Width = cardInDeck.Width;
        Height = cardInDeck.Height;
        Value = cardInDeck.Value;
        SuitValue = cardInDeck.SuitValue;
    }

    public Guid Id { get; set; }
    public int X { get; private init; }
    public int Y { get; private init; }
    public int Width { get; private init; }
    public int Height { get; private init; }
    public int Value { get; private init; }
    public string SuitValue { get; private init; }  // TODO: enum
    public bool FaceDown { get; private init; }

    public Guid PlayerId { get; set;}
    
    // [ForeignKey("player_fk")]
    public virtual PlayerEntity.Player Player { get; set; }

    public static Card Create(Guid id, int x, int y, int width, int height, int value, string suitValue) =>
        new Card(id: id, x: x, y: y, width: width, height: height, value: value, suitValue: suitValue);

    public static Card Create(Guid id, Guid playerId, CardInDeck cardInDeck) =>
        new (id, playerId, cardInDeck);

}