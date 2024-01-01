using App.Application.Extensions;
using App.Application.Identity.Repositories;
using App.Application.Repositories;
using App.Application.Repositories.RoomRepository;
using App.Application.Repositories.UnitOfWork;
using App.Contracts.Data;
using App.Contracts.Mapper;
using App.Contracts.Responses;
using App.Domain.Entities;
using Mediator;
using Microsoft.Extensions.Logging;
using PlayerDto = App.Contracts.Data.PlayerDto;
using RoomDto = App.Contracts.Data.RoomDto;

namespace App.Application.Queries;

public class GetInitDataHandler : IQueryHandler<GetInitDataQuery, InitDataResponse>
{
    private readonly ILogger<GetInitDataHandler> _logger;
    private readonly IPlayerRepository _playerRepository;
    private readonly IPlayerHistoryRepository _playerHistoryRepository;
    private readonly IPlayerInfoRepository _playerInfoRepository;
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IAppUnitOfWork _appUnitOfWork;
    private readonly IPublisher _publisher;

    public GetInitDataHandler(ILogger<GetInitDataHandler> logger, IPlayerRepository playerRepository,
        IPlayerHistoryRepository playerHistoryRepository, IPlayerInfoRepository playerInfoRepository,
        IRoomRepository roomRepository, IAppUnitOfWork appUnitOfWork,
        IApplicationUserRepository applicationUserRepository, IPublisher publisher)
    {
        _logger = logger;
        _playerRepository = playerRepository;
        _playerHistoryRepository = playerHistoryRepository;
        _playerInfoRepository = playerInfoRepository;
        _roomRepository = roomRepository;
        _appUnitOfWork = appUnitOfWork;
        _applicationUserRepository = applicationUserRepository;
        _publisher = publisher;
    }


    public async ValueTask<InitDataResponse> Handle(GetInitDataQuery query, CancellationToken cT)
    {
        query.Deconstruct(out Guid playerId);
        
        var playerResult = await _playerRepository.GetPlayerByIdAsNoTrackingAsync(playerId, cT);
        playerResult.TryFromResult(out PlayerDto? playerDto, out _);

        // if (!playerResult.TryFromResult(out PlayerDto? playerDto, out var errors))
        // {
        //     foreach (var error in errors) _logger.LogError(error.Message);
        //     
        //     await _publisher.Publish(new NotificationResponse(
        //             NotificationId: Guid.NewGuid(),
        //             NotificationText: "An error occurred while updating data, please try again."),
        //         cT);
        //
        //     // return new InitDataResponse(null, null, null, null);
        // }
        
        
        var playerHistory = await _playerHistoryRepository.GetFirstFiveRecordsAsNoTrackingAsync(playerId, cT);
        var playerInfo = await _playerInfoRepository.GetPlayerInfoByIdAsNoTrackingAsync(playerId, cT);
        var rooms = await _roomRepository.GetFirstPageAsNoTrackingAsync(cT);

        var playerInfoResponse = PlayerMapper.MapPlayerInfoToPlayerInfoResponse(playerInfo);

        // PlayerDto? playerDto =
        //     player is null
        //         ? null
        //         : PlayerMapper.MapPlayerToPlayerDto(player);


        IEnumerable<PlayerHistoryResponse>? playerHistoryDto =
            playerHistory is null
                ? null
                : PlayerMapper.MapManyPlayerHistoryToManyPlayerHistoryResponse(playerHistory);


        IEnumerable<RoomInLobbyDto>? roomsDto =
            rooms is null
                ? null
                : RoomMapper.MapManyRoomsToManyRoomsInLobbyDto(rooms);
        
        return new InitDataResponse(
            Player: playerDto,
            PlayerHistories: playerHistoryDto,
            PlayerInfo: playerInfoResponse,
            Rooms: roomsDto);
    }
}
