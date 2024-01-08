using App.Domain.Entities.RoomEntity;
using App.Domain.Primitives;

namespace App.Domain.Entities;

public sealed class CardsDeck : Entity
{
    private readonly List<CardInDeck> _deck;
    
    private CardsDeck(
        Guid id,
        List<CardInDeck> deck,
        Guid roomId) : base(id)
    {
        _deck = deck;
    }

    public IReadOnlyCollection<CardInDeck> Deck => _deck;

    public Guid RoomId { get; private init; }
    public Room Room { get; private init; }

    public CardsDeck Create(Guid id, List<CardInDeck> deck, Guid roomId, Room room)
    {
        return new CardsDeck(
            id: id,
            deck: deck,
            roomId: roomId)
        {
            Room = room
        };
    }

    public Card GetNextCard(Guid cardId, Guid playerId)
    {
        var cardInDeck = _deck.FirstOrDefault();
        _deck.Remove(cardInDeck);

        return Card.Create(id: cardId, playerId: playerId, cardInDeck: cardInDeck);
    }
}