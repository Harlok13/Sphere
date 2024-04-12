using Grpc.Core;
using Identity.Application.Repositories.UnitOfWork;
using Identity.Application.Services;
using IdentityService;

namespace Identity.API.Services;

public class IdentityService : global::IdentityService.Identity.IdentityBase
{
    private readonly ILogger<IdentityService> _logger;
    // private readonly IIdentityUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;

    public IdentityService(
        ILogger<IdentityService> logger,
        // IIdentityUnitOfWork unitOfWork,
        IJwtService jwtService)
    {
        _logger = logger;
        // _unitOfWork = unitOfWork;
        _jwtService = jwtService;
    }

    public override async Task<AuthenticateResponse> Authenticate(AuthenticateRequest request, ServerCallContext context)
    {
        _logger.LogDebug("Auth request\nEmail: {@Email}\nPassword: {@Password}",
            request.Email,
            request.Password);

        // var managedUser = await _unitOfWork.ApplicationUserRepository.GetManagedUserByEmailAsync(request.Email);
        //
        // if (managedUser == null)
        // {
        //     // return BadRequest("Bad credentials");
        //     throw new Exception();  // TODO: ex
        // }
        //
        // var isPasswordValid = await _unitOfWork.ApplicationUserRepository.CheckManagedUserPasswordAsync(managedUser, request.Password);
        //
        // if (!isPasswordValid)
        // {
        //     // return BadRequest("Bad credentials");
        //     _logger.LogDebug(isPasswordValid.ToString(), "password");
        //     throw new Exception();  // TODO: ex
        // }
        //
        // var user = await _unitOfWork.ApplicationUserRepository.GetUserByEmailAsync(request.Email);
        //
        // if (user is null)
        // {
        //     // return Unauthorized();
        //     throw new Exception();  // TODO: ex
        // }
        //
        // var roleIds = await _unitOfWork.ApplicationUserRepository.GetRoleIdsAsync(user);
        // var roles = await _unitOfWork.ApplicationUserRepository.GetRoleNamesAsync(roleIds);
        //
        // var accessToken = _jwtService.GetJwtToken(user, roles);
        // user.RefreshToken = _jwtService.GenerateRefreshToken();
        // user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtService.RefreshTokenValidityInDays);
        //
        // await _unitOfWork.SaveChangesAsync();
        
        
        
        // var playerInfoResult = await _unitOfWork.PlayerInfoRepository.GetPlayerInfoByIdAsync(user.Id, cT);
        // if (!playerInfoResult.TryFromResult(out PlayerInfo? playerInfo, out var playerInfoErrors))
        // {
        //     // TODO: finish
        // }
        // var statisticResponse = PlayerMapper.MapPlayerInfoToPlayerInfoResponse(playerInfo!);

        var playerInfo = new PlayerInfoResponse
        {
            Id = "string",
            AvatarUrl = "url",
            PlayerName = "player name",
            Matches = 3,
            Loses = 2,
            Wins = 5,
            Draws = 10,
            AllExp = 200,
            CurrentExp = 12,
            TargetExp = 20,
            Money = 100,
            Level = 12,
            Has21 = 1
        };
        
        return new AuthenticateResponse {
            PlayerId = "id",
            PlayerName = "name",
            Email = request.Email,
            Token = "token",
            RefreshToken = "refresh token",
            PlayerInfo = playerInfo };

        // return new AuthenticateResponse {
        //     PlayerId = user.Id.ToString(),
        //     PlayerName = user.UserName,
        //     Email = user.Email,
        //     Token = accessToken,
        //     RefreshToken = user.RefreshToken,
        //     PlayerInfo = playerInfo };
    }
    

    public override Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
    {
        return base.Register(request, context);
    }

    public override Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request, ServerCallContext context)
    {
        return base.RefreshToken(request, context);
    }

    public override Task<RevokeResponse> Revoke(RevokeRequest request, ServerCallContext context)
    {
        return base.Revoke(request, context);
    }

    public override Task<RevokeAllResponse> RevokeAll(RevokeAllRequest request, ServerCallContext context)
    {
        return base.RevokeAll(request, context);
    }
}