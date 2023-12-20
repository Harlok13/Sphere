using App.Application.Identity.Repositories;
using App.Application.Mapper;
using App.Application.Repositories;
using App.Application.Repositories.RoomRepository;
using App.Application.Repositories.UnitOfWork;
using App.Contracts.Responses;
using App.Domain.Entities;
using Mediator;
using Microsoft.Extensions.Logging;

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

    public GetInitDataHandler(ILogger<GetInitDataHandler> logger, IPlayerRepository playerRepository,
        IPlayerHistoryRepository playerHistoryRepository, IPlayerInfoRepository playerInfoRepository,
        IRoomRepository roomRepository, IAppUnitOfWork appUnitOfWork,
        IApplicationUserRepository applicationUserRepository)
    {
        _logger = logger;
        _playerRepository = playerRepository;
        _playerHistoryRepository = playerHistoryRepository;
        _playerInfoRepository = playerInfoRepository;
        _roomRepository = roomRepository;
        _appUnitOfWork = appUnitOfWork;
        _applicationUserRepository = applicationUserRepository;
    }


    public async ValueTask<InitDataResponse> Handle(GetInitDataQuery query, CancellationToken cT)
    {
        query.Deconstruct(out Guid playerId);
        _logger.LogInformation($"Player id: {playerId}");
        
        var player = await _playerRepository.GetPlayerByIdAsync(playerId, cT);
        var playerHistory = await _playerHistoryRepository.GetFirstFiveRecordsAsync(playerId, cT);
        var playerInfo = await _playerInfoRepository.GetPlayerInfoByIdAsync(playerId, cT);
        var rooms = await _roomRepository.GetFirstPageAsync(cT);

        // if (player is null)
        // {
        //     var user = await _applicationUserRepository.GetUserByIdAsync(playerId, cT);
        //     if (user is null) throw new Exception();
        //     player = Player.Create(playerId, user.UserName!);
        // }

        // player ??= Player.Create(playerId,
        //     _applicationUserRepository.GetUserByIdAsync(playerId, cT).GetAwaiter().GetResult().UserName!);

        // playerInfo ??= PlayerInfo.Create(Guid.NewGuid(), playerId);  // TODO: finish validation
        var playerInfoResponse = PlayerMapper.MapPlayerInfoToPlayerInfoResponse(playerInfo);

        PlayerResponse? playerResponse =
            player is null
                ? null
                : PlayerMapper.MapPlayerToPlayerResponse(player);


        IEnumerable<PlayerHistoryResponse>? playerHistoryResponse =
            playerHistory is null
                ? null
                : PlayerMapper.MapManyPlayerHistoryToManyPlayerHistoryResponse(playerHistory);


        IEnumerable<RoomResponse>? roomsResponse =
            rooms is null
                ? null
                : RoomMapper.MapManyRoomsToManyRoomsResponse(rooms);
        
        var user = await _applicationUserRepository.GetUserByIdAsync(playerId, cT);
        // var userResponse = ApplicationUserMapper.MapApplicationUserToApplicationUserResponse(user);
        
        return new InitDataResponse(
            // UserResponse: userResponse,
            PlayerResponse: playerResponse,
            PlayerHistoriesResponse: playerHistoryResponse,
            PlayerInfoResponse: playerInfoResponse,
            RoomsResponse: roomsResponse);
    }
}
