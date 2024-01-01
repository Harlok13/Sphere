using System.Collections.Immutable;
using App.Application.Repositories;
using App.Application.Repositories.RoomRepository;
using App.Application.Repositories.UnitOfWork;
using App.Application.Services.Interfaces;
using App.Domain.Entities.PlayerEntity;
using App.SignalR.Commands.RoomCommands;
using App.SignalR.Hubs;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace App.Application.Handlers.RoomHandlers;

public class EndGameHandler : ICommandHandler<EndGameCommand, bool>
{
    private readonly ILogger<EndGameHandler> _logger;
    private readonly IPlayerRepository _playerRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IHubContext<GlobalHub, IGlobalHub> _hubContext;
    private readonly IMediator _mediator;

    public EndGameHandler(
        ILogger<EndGameHandler> logger,
        IPlayerRepository playerRepository,
        IRoomRepository roomRepository,
        IAppUnitOfWork unitOfWork,
        IHubContext<GlobalHub, IGlobalHub> hubContext,
        IMediator mediator)
    {
        _logger = logger;
        _playerRepository = playerRepository;
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
        _hubContext = hubContext;
        _mediator = mediator;
    }
    
    public async ValueTask<bool> Handle(EndGameCommand command, CancellationToken cT)
    {
        command.Deconstruct(out Guid roomId);

        var room = await _unitOfWork.RoomRepository.GetByIdAsync(roomId, cT);
        var players = room.Players.ToArray();
        var bank = room.Bank;
        room.ResetBank();

        var losingPlayers = players.Where(p => p.Score > 21).ToArray();
        var has21Players = players.Where(p => p.Score == 21).ToImmutableArray();
        var hasLt21Players = players.Where(p => p.Score < 21).ToArray();

        if (has21Players.Length == 1)
        {
            var player = has21Players.First();
            var playerInfo = await _unitOfWork.PlayerInfoRepository.GetPlayerInfoByIdAsync(player.Id, cT);
            playerInfo.Win();
            player.EndGame();
            player.IncreaseMoney(bank);
            await SetLoseAsync(losingPlayers, hasLt21Players);
            return await _unitOfWork.SaveChangesAsync(cT);
        }
        else if (has21Players.Length > 1)
        {
            var hasGold21Players = has21Players
                .Where(p => p.Cards.Where(c => c.Value == 11).ToImmutableArray().Length == 2)
                .ToImmutableArray();

            switch (hasGold21Players.Length)
            {
                case 1:
                    var playerHasGold21 = has21Players.First();
                    var playerInfoHas21Gold = await _unitOfWork.PlayerInfoRepository.GetPlayerInfoByIdAsync(playerHasGold21.Id, cT);
                    playerHasGold21.EndGame();
                    playerHasGold21.IncreaseMoney(bank);
                    playerInfoHas21Gold.Win();
                    await SetLoseAsync(players.Where(p => p.Id != playerHasGold21.Id).ToArray());
                    return await _unitOfWork.SaveChangesAsync(cT);
                
                case 2:
                    foreach (var player in hasGold21Players)
                    {
                        var winningsMoney = bank / hasGold21Players.Length;
                        var playerInfo = await _unitOfWork.PlayerInfoRepository.GetPlayerInfoByIdAsync(player.Id, cT);
                        player.EndGame();
                        player.IncreaseMoney(winningsMoney);
                        playerInfo.Draw();
                        await SetLoseAsync(players.Where(p => p.Id != hasGold21Players[0].Id || p.Id != hasGold21Players[1].Id).ToArray());
                    }
                    return await _unitOfWork.SaveChangesAsync(cT);
            }

            foreach (var player in has21Players)
            {
                var winningsMoney = bank / has21Players.Length;
                var playerInfo = await _unitOfWork.PlayerInfoRepository.GetPlayerInfoByIdAsync(player.Id, cT);
                player.EndGame();
                player.IncreaseMoney(winningsMoney);
                playerInfo.Draw();
                await SetLoseAsync(losingPlayers, hasLt21Players);
            }

            return await _unitOfWork.SaveChangesAsync(cT);
        }

        if (hasLt21Players.Length > 0)
        {
            var maxScore = hasLt21Players.Max(p => p.Score);
            var hasMaxScorePlayers = hasLt21Players.Where(p => p.Score == maxScore).ToImmutableArray();

            if (hasMaxScorePlayers.Length == 1)
            {
                var player = hasMaxScorePlayers.First();
                var playerInfo =
                    await _unitOfWork.PlayerInfoRepository.GetPlayerInfoByIdAsync(player.Id, cT);
                player.EndGame();
                player.IncreaseMoney(bank);
                playerInfo.Win();
                await SetLoseAsync(players.Where(p => p.Id != player.Id).ToArray());
                return await _unitOfWork.SaveChangesAsync(cT);
            }

            foreach (var player in hasMaxScorePlayers)
            {
                var winningsMoney = bank / hasMaxScorePlayers.Length;
                var playerInfo = await _unitOfWork.PlayerInfoRepository.GetPlayerInfoByIdAsync(player.Id, cT);
                player.EndGame();
                player.IncreaseMoney(winningsMoney);
                playerInfo.Draw();
                await SetLoseAsync(losingPlayers);
                return await _unitOfWork.SaveChangesAsync(cT);
            }
        }
        
        await SetLoseAsync(players);
        
        _logger.LogInformation("End game.");
        return true;
    }

    private async Task SetLoseAsync(params Player[][] args)
    {
        foreach (var players in args)
        {
            foreach (var player in players)
            {
                var playerInfo = await _unitOfWork.PlayerInfoRepository.GetPlayerInfoByIdAsync(player.Id);
                player.EndGame();
                playerInfo.Lose();
            }
        }
    }
}