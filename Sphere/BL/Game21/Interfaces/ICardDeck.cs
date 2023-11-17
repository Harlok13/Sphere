using Sphere.BL.Models.Game21;

namespace Sphere.BL.Game21.Interfaces;

public interface ICardDeck
{
    public CardModel GetNextCard();
    
    public void ResetDeck();
}