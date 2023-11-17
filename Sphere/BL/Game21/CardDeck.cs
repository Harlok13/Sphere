using Newtonsoft.Json;
using Sphere.BL.Game21.Interfaces;
using Sphere.BL.Models.Game21;

namespace Sphere.BL.Game21;

public class CardDeck : ICardDeck
{
    private readonly List<CardModel> _cardDeck;
    private int _cardDeckIndex = 0;  
    private const int CardDeckSize = 52;

    public CardDeck()
    {
        _cardDeck = CreateCardDeck();
    }

    public CardModel GetNextCard()
    {
        var cardsGenerator = CardsGenerator();
        cardsGenerator.MoveNext();

        _cardDeckIndex++;
        
        return cardsGenerator.Current;
    }

    public void ResetDeck()
    {
        _cardDeckIndex = 0;
        ShuffleCardDeck(_cardDeck);
    }
    
    private List<CardModel> CreateCardDeck()
    {
        var data = ReadJson();
        var cardDeck = new List<CardModel>();

        if (data == null) throw new Exception("Data is empty!");  // TODO: custom ex
        
        foreach (var cardDataIndex in Enumerable.Range(0, CardDeckSize))
        {
            cardDeck.Add(DeserializeCard(data[$"{cardDataIndex}"]["frame"]));
        }
        
        ShuffleCardDeck(cardDeck);

        return cardDeck;
    }

    private void ShuffleCardDeck(List<CardModel> cardDeck)
    {
        var random = new Random();
        var index = cardDeck.Count;

        while (index > 1)
        {
            index--;
            var newIndex = random.Next(index + 1);
            
            (cardDeck[index], cardDeck[newIndex]) = (cardDeck[newIndex], cardDeck[index]);
        }
    }

    private Dictionary<string, dynamic>? ReadJson()
    {
        var json = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "JsonData", "cards.json"));  
        
        return JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json);
    }

    private CardModel DeserializeCard(dynamic currentCard)
    {
        return JsonConvert.DeserializeObject<CardModel>(currentCard.ToString());
        
    }

    private IEnumerator<CardModel> CardsGenerator()
    {
        while (_cardDeckIndex < CardDeckSize)
        {
            yield return _cardDeck[_cardDeckIndex];
        }
    }
}