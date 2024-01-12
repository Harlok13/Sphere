using App.Domain.Entities;

namespace App.Application.Services.Interfaces;

public interface ICardsDeckService
{
    IEnumerable<CardInDeck> Create();
}