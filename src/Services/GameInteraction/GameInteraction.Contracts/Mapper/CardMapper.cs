using GameInteraction.Contracts.Data;
using GameInteraction.Domain.Entities;

namespace GameInteraction.Contracts.Mapper;

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