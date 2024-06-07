using AutoMapper;
using Core.Extensions;
using Core.Shared;
using Grpc.Core;
using UserInteraction.Application.Repositories.UnitOfWork;
using UserInteraction.Domain.Entities.PlayerInfoEntity;
using GrpcUserInteractionService;

namespace UserInteraction.API.Services;

public class UserInteractionService : global::GrpcUserInteractionService.UserInteraction.UserInteractionBase
{
    private readonly ILogger<UserInteractionService> _logger;
    private readonly IUserInteractionUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public UserInteractionService(
        ILogger<UserInteractionService> logger,
        IUserInteractionUnitOfWork unitOfWork, 
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public override Task<GrpcAddToFriendsResponse> AddToFriends(GrpcAddToFriendsRequest request, ServerCallContext context)
    {
        _logger.LogInformation("The method {InvokedMethod} was invoked with args: {@Request}",
            nameof(AddToFriends),
            request);
        
        return Task.FromResult(new GrpcAddToFriendsResponse
        {
            Name = request.Name
        });
    }

    public override async Task<GrpcCreatePlayerInfoResponse> CreatePlayerInfo(GrpcCreatePlayerInfoRequest request, ServerCallContext context)
    {
        _logger.LogInformation("The method {@InvokedMethod} was invoked with args: {@Request}",
            nameof(CreatePlayerInfo),
            request);
        
        Guid userId = Guid.Parse(request.UserId);
        await _unitOfWork.PlayerInfoRepository.CreatePlayerInfoAsync(userId, request.PlayerName, default);
        
        await _unitOfWork.SaveChangesAsync();

        return new GrpcCreatePlayerInfoResponse();
    }

    public override async Task<GrpcGetPlayerInfoResponse> GetPlayerInfo(GrpcGetPlayerInfoRequest request, ServerCallContext context)
    {
        _logger.LogInformation("The method {@InvokedMethod} was invoked with args: {@Request}",
            nameof(GetPlayerInfo),
            request);
        
        Guid playerId = Guid.Parse(request.PlayerId);
        
        Result<PlayerInfo> playerInfoResult = await _unitOfWork.PlayerInfoRepository.GetPlayerInfoByIdAsync(playerId);
        if (playerInfoResult.TryFromResult(out PlayerInfo? playerInfo, out var playerInfoErrors))
        {
            foreach (var error in playerInfoErrors) _logger.LogError(error.Message);
        }

        GrpcGetPlayerInfoResponse playerInfoResponse = _mapper.Map<GrpcGetPlayerInfoResponse>(playerInfo);
        
        return playerInfoResponse;
    }
}