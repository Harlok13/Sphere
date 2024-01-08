using App.Domain.Entities;

namespace App.Application.Services.Interfaces;

public interface ICardsDeckService
{
    Task<Card> GetNextCardAsync(Guid roomId, Guid playerId, CancellationToken cT);

    Task ResetAsync(Guid roomId, CancellationToken cT);

    IEnumerable<CardInDeck> Create();
}