// using App.Application.Repositories;
// using App.Application.Services.Interfaces;
// using App.Domain.Entities;
// using App.Domain.Entities.RoomEntity;
// using Microsoft.Extensions.Logging;
//
// namespace App.Application.Services;
//
// public class Game21Service : IGame21Service
// {
//     private readonly ILogger<Game21Service> _logger;
//     private readonly ICardsDeckService _cardsDeckService;
//     private readonly IPlayerInfoRepository _playerInfoRepository;
//
//     public Game21Service(ILogger<Game21Service> logger, ICardsDeckService cardsDeckService, IPlayerInfoRepository playerInfoRepository)
//     {
//         _logger = logger;
//         _cardsDeckService = cardsDeckService;
//         _playerInfoRepository = playerInfoRepository;
//     }
//
//     public void StartGame(Room room)
//     {
//         foreach (var player in room.Players)
//         {
//             var card = Card.Create(Guid.NewGuid(), player.Id, _cardsDeckService.GetNextCardAsync());
//             player.AddNewCard(card);
//         }
//     }
// }