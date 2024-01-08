using App.Application.Repositories;
using App.Application.Services;
using App.Application.Services.Interfaces;
using App.Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace App.Infra.Repositories;

/* Temporary solution */
public class CardsDeckRepository : ICardsDeckRepository
{
    private readonly IDistributedCache _redis;
    // private readonly ICardsDeckService _cardsDeck;

    public CardsDeckRepository(
        IDistributedCache redis
        // ICardsDeckService cardsDeck
        )
    {
        _redis = redis;
        // _cardsDeck = cardsDeck;
    }

    // public async Task<CardInDeck> GetNextCardAsync(Guid roomId, CancellationToken cT)
    // {
    //     var cardsDeckString = await _redis.GetStringAsync($"deck_{roomId}", cT);
    //     var cardsDeck = JsonConvert.DeserializeObject<CardsDeckService>(cardsDeckString);
    //     return cardsDeck.GetNextCardAsync();
    // }

    public async Task AddCardsDeckAsync(Guid roomId, IEnumerable<CardInDeck> cardsDeck, CancellationToken cT)
    {
        // var cardsDeck = new CardsDeckService();
        var value = JsonSerializer.Serialize(cardsDeck);
        await _redis.SetStringAsync($"deck_{roomId}", value, cT);
    }

    public async Task<ICollection<CardInDeck>> GetCardsDeckAsync(Guid roomId, CancellationToken cT)
    {
        var cardsDeckString = await _redis.GetStringAsync($"deck_{roomId}", cT);
        return JsonSerializer.Deserialize<List<CardInDeck>>(cardsDeckString);
    }

    public async Task RemoveCardsDeck(Guid roomId, CancellationToken cT)
    {
        await _redis.RemoveAsync($"deck_{roomId}", cT);
    }

    public async Task SaveChangesAsync(Guid roomId, IEnumerable<CardInDeck> cardInDecks, CancellationToken cT)
    {
        var value = JsonSerializer.Serialize(cardInDecks);
        await _redis.SetStringAsync($"deck_{roomId}", value, cT);
    }

    // public async Task ShuffleCardsDeckAsync(Guid roomId, CancellationToken cT)
    // {
    //     var cardsDeckString = await _redis.GetStringAsync($"deck_{roomId}", cT);
    //     var cardsDeck = JsonConvert.DeserializeObject<CardsDeckService>(cardsDeckString);
    //     cardsDeck.ResetAsync();
    //     
    //     var value = JsonSerializer.Serialize(cardsDeck);
    //     await _redis.SetStringAsync($"deck_{roomId}", value, cT);
    // }
}