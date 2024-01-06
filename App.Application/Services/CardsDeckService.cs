using App.Application.Repositories;
using App.Application.Services.Interfaces;
using App.Domain.Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace App.Application.Services;

public sealed class CardsDeckService : ICardsDeckService
{
    private const int CardDeckSize = 52;

    private readonly ILogger<CardsDeckService> _logger;
    private readonly ICardsDeckRepository _cardsDeckRepository;
    
    public CardsDeckService(ICardsDeckRepository cardsDeckRepository, ILogger<CardsDeckService> logger)
    {
        _cardsDeckRepository = cardsDeckRepository;
        _logger = logger;
    }

    public async Task<Card> GetNextCardAsync(Guid roomId, Guid playerId, CancellationToken cT)
    {
        var cardsDeck = await _cardsDeckRepository.GetCardsDeckAsync(roomId, cT);
        var cardInDeck = cardsDeck.FirstOrDefault();  // TODO: finish 
        cardsDeck.Remove(cardInDeck);

        await _cardsDeckRepository.SaveChangesAsync(roomId: roomId, cardsDeck: cardsDeck, cT);

        return Card.Create(Guid.NewGuid(), playerId, cardInDeck);
    }

    public async Task ResetAsync(Guid roomId, CancellationToken cT)
    {
        await _cardsDeckRepository.RemoveCardsDeck(roomId, cT);
        // await CreateAsync(roomId, cT);
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
        // await _cardsDeckRepository.AddCardsDeckAsync(roomId: roomId, cardsDeck: cardsDeck, cT);
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

        return JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json);
    }

    private CardInDeck DeserializeCard(dynamic currentCard) =>
        JsonConvert.DeserializeObject<CardInDeck>(currentCard.ToString());
}