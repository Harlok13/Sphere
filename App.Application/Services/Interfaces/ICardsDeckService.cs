using App.Domain.Entities;

namespace App.Application.Services.Interfaces;

public interface ICardsDeckService
{
    Task<CardInDeck> GetNextCardAsync(Guid roomId, CancellationToken cT);

    Task ResetAsync(Guid roomId, CancellationToken cT);

    Task CreateAsync(Guid roomId, CancellationToken cT);
}