using App.Domain.Entities;

namespace App.Application.Repositories;

public interface ICardsDeckRepository
{
    Task AddCardsDeckAsync(Guid roomId, IEnumerable<CardInDeck> cardsDeck, CancellationToken cT);

    Task<ICollection<CardInDeck>> GetCardsDeckAsync(Guid roomId, CancellationToken cT);

    Task RemoveCardsDeck(Guid roomId, CancellationToken cT);

    Task SaveChangesAsync(Guid roomId, IEnumerable<CardInDeck> cardsDeck, CancellationToken cT);
}