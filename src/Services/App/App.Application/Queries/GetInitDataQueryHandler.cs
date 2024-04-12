using App.Application.Extensions;
using App.Application.Identity.Repositories;
using App.Application.Repositories;
using App.Application.Repositories.RoomRepository;
using App.Application.Repositories.UnitOfWork;
using App.Contracts.Data;
using App.Contracts.Mapper;
using App.Contracts.Responses;
using App.Domain.Entities;
using App.Domain.Shared;
using App.Domain.Shared.ResultImplementations;
using Mediator;
using Microsoft.Extensions.Logging;
using PlayerDto = App.Contracts.Data.PlayerDto;
using RoomDto = App.Contracts.Data.RoomDto;

namespace App.Application.Queries;

public class GetInitDataQueryHandler : IQueryHandler<GetInitDataQuery, InitDataResponse>
{
    private readonly ILogger<GetInitDataQueryHandler> _logger;
    private readonly IAppUnitOfWork _unitOfWork;

    public GetInitDataQueryHandler(
        ILogger<GetInitDataQueryHandler> logger,
        IAppUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }


    public async ValueTask<InitDataResponse> Handle(GetInitDataQuery query, CancellationToken cT) 
    {
        query.Deconstruct(out Guid playerId);
        
        var playerResult = await _unitOfWork.PlayerRepository.GetPlayerByIdAsNoTrackingAsync(playerId, cT);
        playerResult.TryFromResult(out PlayerDto? playerDto, out _);

        var playerHistoryDtos = await _unitOfWork.PlayerHistoryRepository.GetFirstFiveRecordsAsNoTrackingAsync(playerId, cT);
        var roomDtos = await _unitOfWork.RoomRepository.GetFirstPageAsNoTrackingAsync(cT);
        
        var playerInfoResult = await _unitOfWork.PlayerInfoRepository.GetPlayerInfoByIdAsNoTrackingAsync(playerId, cT);
        playerInfoResult.TryFromResult(out PlayerInfoDto? playerInfoDto, out _);

        _logger.LogInformation(
            "Initialization data was sent to player with id \"{PlayerId}\". Data: {Data}.",
            playerId,
            new {Player = playerDto, PlayerHistories = playerHistoryDtos, PlayerInfo = playerInfoDto, rooms = roomDtos});
        
        return new InitDataResponse(
            Player: playerDto,
            PlayerHistories: playerHistoryDtos,
            PlayerInfo: playerInfoDto,
            Rooms: roomDtos);
    }
}
