using GameInteraction.Domain.Entities;

namespace GameInteraction.Application.Services.Interfaces;

public interface ICardsDeckService
{
    IEnumerable<CardInDeck> Create();
}