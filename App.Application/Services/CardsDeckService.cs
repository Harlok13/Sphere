using System.Text.Json;
using App.Application.Services.Interfaces;
using App.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace App.Application.Services;

public sealed class CardsDeckService : ICardsDeckService
{
    private const int CardDeckSize = 52;

    private readonly ILogger<CardsDeckService> _logger;
    
    public CardsDeckService(ILogger<CardsDeckService> logger)
    {
        _logger = logger;
    }

    public IEnumerable<CardInDeck> Create()
    {
        var data = ReadJson();
        var cardsDeck = new List<CardInDeck>();

        if (data == null) throw new Exception("Data is empty!");  // TODO: custom ex

        foreach (var cardDataIndex in Enumerable.Range(0, CardDeckSize))
        {
            cardsDeck.Add(DeserializeCard(data[$"{cardDataIndex}"]["frame"]));
        }
        
        Shuffle(cardsDeck);

        return cardsDeck;
    }

    private void Shuffle(List<CardInDeck> cardsDeck)
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

    private Dictionary<string, dynamic>? ReadJson()  // TODO: add cache
    {
        var json = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "JsonData", "cards.json"));

        return JsonSerializer.Deserialize<Dictionary<string, dynamic>>(json);
    }

    private CardInDeck DeserializeCard(dynamic currentCard) =>
        JsonSerializer.Deserialize<CardInDeck>(currentCard.ToString());
}