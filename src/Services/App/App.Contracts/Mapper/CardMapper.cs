using App.Contracts.Data;
using App.Contracts.Responses;
using App.Domain.Entities;

namespace App.Contracts.Mapper;

public class CardMapper
{
    public static CardDto MapCardToCardDto(Card entity)
    {
        return new CardDto(
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