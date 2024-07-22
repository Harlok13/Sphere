using App.Application.Repositories.UnitOfWork;
using App.Contracts.Data;
using App.Contracts.Responses;
using App.GrpcClient.UserInteractionClient;
using AutoMapper;
using Core.Extensions;
using GrpcUserInteractionClient;
using Mediator;
using Microsoft.Extensions.Logging;
using PlayerDto = App.Contracts.Data.PlayerDto;

namespace App.Application.Queries;

public class GetInitDataQueryHandler : IQueryHandler<GetInitDataQuery, InitDataResponse>
{
    private readonly ILogger<GetInitDataQueryHandler> _logger;
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IUserInteractionClient _userInteractionClient;
    private readonly IMapper _mapper;

    public GetInitDataQueryHandler(
        ILogger<GetInitDataQueryHandler> logger,
        IAppUnitOfWork unitOfWork, 
        IUserInteractionClient userInteractionClient, 
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _userInteractionClient = userInteractionClient;
        _mapper = mapper;
    }

    // TODO: ref
    public async ValueTask<InitDataResponse> Handle(GetInitDataQuery query, CancellationToken cT) 
    {
        query.Deconstruct(out Guid playerId);
        
        var playerResult = await _unitOfWork.PlayerRepository.GetPlayerByIdAsNoTrackingAsync(playerId, cT);
        playerResult.TryFromResult(out PlayerDto? playerDto, out _);

        var playerHistoryDtos = await _unitOfWork.PlayerHistoryRepository.GetFirstFiveRecordsAsNoTrackingAsync(playerId, cT);
        var roomDtos = await _unitOfWork.RoomRepository.GetFirstPageAsNoTrackingAsync(cT);
        
        // var playerInfoResult = await _unitOfWork.PlayerInfoRepository.GetPlayerInfoByIdAsNoTrackingAsync(playerId, cT);
        // playerInfoResult.TryFromResult(out PlayerInfoDto? playerInfoDto, out _);
        var grpcPlayerInfo =
            await _userInteractionClient.GetPlayerInfoAsync(new GrpcGetPlayerInfoRequest { PlayerId = playerId.ToString() }, cT);
        var playerInfoDto = _mapper.Map<PlayerInfoDto>(grpcPlayerInfo);

        _logger.LogInformation(
            "Initialization data was sent to player with id \"{@PlayerId}\". Data: {@Data}.",
            playerId,
            new {Player = playerDto, PlayerHistories = playerHistoryDtos, PlayerInfo = playerInfoDto, rooms = roomDtos});
        
        return new InitDataResponse(
            Player: playerDto,
            PlayerHistories: playerHistoryDtos,
            PlayerInfo: playerInfoDto,
            Rooms: roomDtos);
    }
}
