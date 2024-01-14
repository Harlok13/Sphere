// using App.Application.Services;
// using App.Domain.Entities;
// using Microsoft.Extensions.Caching.Distributed;
// using Newtonsoft.Json;
// using JsonSerializer = System.Text.Json.JsonSerializer;
//
// namespace App.Application.Extensions;
//
// public static class DistributedCacheExtensions
// {
//     public static async Task<CardInDeck> GetNextCard(this IDistributedCache cache, Guid roomId)
//     {
//         var cardsDeckString = await cache.GetStringAsync($"deck_{roomId}");
//         var cardsDeck = JsonConvert.DeserializeObject<CardsDeckService>(cardsDeckString);
//         return cardsDeck.GetNextCardAsync();
//     }
//
//     public static async Task AddCardsDeck(this IDistributedCache cache, Guid roomId)
//     {
//         var cardsDeck = new CardsDeckService();
//         var value = JsonSerializer.Serialize(cardsDeck);
//         await cache.SetStringAsync($"deck_{roomId}", value);
//     }
//
//     public static async Task ShuffleCardsDeck(this IDistributedCache cache, Guid roomId)
//     {
//         var cardsDeckString = await cache.GetStringAsync($"deck_{roomId}");
//         var cardsDeck = JsonConvert.DeserializeObject<CardsDeckService>(cardsDeckString);
//         cardsDeck.ResetAsync();
//         
//         var value = JsonSerializer.Serialize(cardsDeck);
//         await cache.SetStringAsync($"deck_{roomId}", value);
//     }
// }