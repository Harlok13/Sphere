using App.Contracts.Responses;
using App.Domain.Entities;

namespace App.Application.Mapper;

public class CardMapper
{
    public static CardResponse MapCardToCardResponse(Card entity)
    {
        return new CardResponse(
            Id: entity.Id,
            PlayerId: entity.PlayerId,
            X: entity.X,
            Y: entity.Y,
            Width: entity.Width,
            Height: entity.Height,
            Value: entity.Value,
            SuitValue: entity.SuitValue);
    }
}