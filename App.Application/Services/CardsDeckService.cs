using App.Application.Services.Interfaces;
using App.Domain.Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace App.Application.Services;

public sealed class CardsDeckService : ICardsDeckService
{
    private readonly ILogger<CardsDeckService> _logger;
    private readonly List<CardInDeck> _cardsDeck;
    private int _cardsDeckIndex = 0;
    private const int CardDeckSize = 52;
    
    public CardsDeckService(ILogger<CardsDeckService> logger)
    {
        _logger = logger;
        _cardsDeck = CreateCardsDeck();
    }

    public CardInDeck GetNextCard()
    {
        var cardsGenerator = CardsGenerator();
        cardsGenerator.MoveNext();

        _cardsDeckIndex++;

        return cardsGenerator.Current;
    }

    public void ResetDeck()
    {
        _cardsDeckIndex = default;
        ShuffleCardsDeck(_cardsDeck);
    }

    private List<CardInDeck> CreateCardsDeck()
    {
        var data = ReadJson();
        var cardsDeck = new List<CardInDeck>();

        if (data == null) throw new Exception("Data is empty!");  // TODO: custom ex

        // List<Card> cardsDeck = (List<Card>)Enumerable.Range(0, CardDeckSize)
        //     .Select(cardDataIndex => DeserializeCard(data[$"{cardDataIndex}"]["frame"]))
        //     .ToList();
        foreach (var cardDataIndex in Enumerable.Range(0, CardDeckSize))
        {
            cardsDeck.Add(DeserializeCard(data[$"{cardDataIndex}"]["frame"]));
        }
        
        ShuffleCardsDeck(cardsDeck);

        return cardsDeck;
    }

    private void ShuffleCardsDeck(List<CardInDeck> cardsDeck)
    {
        var random = new Random();
        var index = cardsDeck.Count;

        while (index > 1)
        {
            var newIndex = random.Next(index);
            index--;

            (cardsDeck[index], cardsDeck[newIndex]) = (cardsDeck[newIndex], cardsDeck[index]);
        }
    }

    private Dictionary<string, dynamic>? ReadJson()
    {
        var json = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "JsonData", "cards.json"));

        return JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json);
    }

    private CardInDeck DeserializeCard(dynamic currentCard) =>
        JsonConvert.DeserializeObject<CardInDeck>(currentCard.ToString());

    private IEnumerator<CardInDeck> CardsGenerator()
    {
        while (_cardsDeckIndex < CardDeckSize) 
            yield return _cardsDeck[_cardsDeckIndex];
    }
}