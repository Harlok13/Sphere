using System.IdentityModel.Tokens.Jwt;
using Grpc.Core;
using Identity.Application.Repositories.UnitOfWork;
using Identity.Application.Services;
using Identity.Domain.Entities;
using GrpcIdentityService;

namespace Identity.API.Services;

public class IdentityService : global::GrpcIdentityService.Identity.IdentityBase
{
    private readonly ILogger<IdentityService> _logger;
    private readonly IIdentityUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;

    public IdentityService(
        ILogger<IdentityService> logger,
        IIdentityUnitOfWork unitOfWork,
        IJwtService jwtService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
    }

    public override async Task<GrpcAuthenticateResponse> Authenticate(GrpcAuthenticateRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("The method {InvokedMethod} was invoked with args: {@Request}",
            nameof(Authenticate),
            request);

        var managedUser = await _unitOfWork.IdentityUserRepository.GetManagedUserByEmailAsync(request.Email);

        if (managedUser == null)
        {
            // return BadRequest("Bad credentials");
            throw new Exception(); // TODO: ex
        }

        var isPasswordValid =
            await _unitOfWork.IdentityUserRepository.CheckManagedUserPasswordAsync(managedUser, request.Password);

        if (!isPasswordValid)
        {
            // return BadRequest("Bad credentials");
            _logger.LogDebug(isPasswordValid.ToString(), "password");
            throw new Exception(); // TODO: ex
        }

        var user = await _unitOfWork.IdentityUserRepository.GetUserByEmailAsync(request.Email);

        if (user is null)
        {
            // return Unauthorized();
            throw new Exception(); // TODO: ex
        }

        var roleIds = await _unitOfWork.IdentityUserRepository.GetRoleIdsAsync(user);
        var roles = await _unitOfWork.IdentityUserRepository.GetRoleNamesAsync(roleIds);

        var accessToken = _jwtService.GetJwtToken(user, roles);
        user.RefreshToken = _jwtService.GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtService.RefreshTokenValidityInDays);

        await _unitOfWork.SaveChangesAsync();


        // var playerInfoResult = await _unitOfWork.PlayerInfoRepository.GetPlayerInfoByIdAsync(user.Id, cT);
        // if (!playerInfoResult.TryFromResult(out PlayerInfo? playerInfo, out var playerInfoErrors))
        // {
        //     // TODO: finish
        // }
        // var statisticResponse = PlayerMapper.MapPlayerInfoToPlayerInfoResponse(playerInfo!);

        // var playerInfo = new PlayerInfoResponse
        // {
        //     Id = "string",
        //     AvatarUrl = "url",
        //     PlayerName = "player name",
        //     Matches = 3,
        //     Loses = 2,
        //     Wins = 5,
        //     Draws = 10,
        //     AllExp = 200,
        //     CurrentExp = 12,
        //     TargetExp = 20,
        //     Money = 100,
        //     Level = 12,
        //     Has21 = 1
        // };

        // return new AuthenticateResponse
        // {
        //     PlayerId = "id",
        //     PlayerName = "name",
        //     Email = request.Email,
        //     Token = "token",
        //     RefreshToken = "refresh token",
        //     PlayerInfo = playerInfo
        // };

        return new GrpcAuthenticateResponse {
            PlayerId = user.Id.ToString(),
            PlayerName = user.UserName,
            Email = user.Email,
            Token = accessToken,
            RefreshToken = user.RefreshToken };
    }


    public override async Task<GrpcRegisterResponse> Register(GrpcRegisterRequest request, ServerCallContext context)
    {
        _logger.LogInformation("The method {InvokedMethod} was invoked with args: {@Request}",
            nameof(Register),
            request);
        
        var user = new User
        {
            Email = request.Email,
            UserName = request.UserName
        };

        var result = await _unitOfWork.IdentityUserRepository.CreateManagedUserAsync(user, request.Password);
        if (!result.Succeeded)
        {
            foreach (var err in result.Errors)
                _logger.LogError(err.Description);
            throw new Exception(); // TODO: ex
        }

        var findUser = await _unitOfWork.IdentityUserRepository.GetUserByEmailAsync(request.Email);
        if (findUser is null) throw new Exception($"User {request.Email} not found"); // TODO: custom ex

        await _unitOfWork.IdentityUserRepository.AddToRoleAsync(findUser);
        // await _unitOfWork.PlayerInfoRepository.CreatePlayerInfoAsync(findUser.Id, findUser.UserName!);

        // var authCommand = new AuthenticateCommand(
        //     new AuthenticateRequest(request.Email, request.Password));
        // return await _mediator.Send(authCommand, cT);

        await _unitOfWork.SaveChangesAsync();

        return new GrpcRegisterResponse { Email = request.Email, Password = request.Password, UserId = findUser.Id.ToString() };

        // var authRequest = new AuthenticateRequest { Email = request.Email, Password = request.Password };
        // return await Authenticate(authRequest, context);
    }

    public override async Task<GrpcRefreshTokenResponse> RefreshToken(GrpcRefreshTokenRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("The method {InvokedMethod} was invoked with args: {@Request}",
            nameof(RefreshToken),
            request);
        
        if (request is null)
        {
            throw new Exception(); // TODO: ex
            // return BadRequest("Invalid client request");
        }

        var accessToken = request.AccessToken;
        var refreshToken = request.RefreshToken;
        var principal = _jwtService.GetPrincipalFromExpiredToken(accessToken);

        if (principal == null)
        {
            // return BadRequest("Invalid access token or refresh token");
            throw new Exception(); // TODO: ex
        }

        var username = principal.Identity.Name;
        var user = await _unitOfWork.IdentityUserRepository.GetManagedUserByNameAsync(username);

        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            // return BadRequest("Invalid access token or refresh token");
            throw new Exception(); // TODO: ex
        }

        var newAccessToken = _jwtService.CreateToken(principal.Claims.ToArray());
        var newRefreshToken = _jwtService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _unitOfWork.IdentityUserRepository.UpdateManagedUserAsync(user);

        // return new ObjectResult(new
        // {
        //     accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
        //     refreshToken = newRefreshToken
        // });
        return new GrpcRefreshTokenResponse
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            RefreshToken = newRefreshToken
        };
    }

    public override async Task<GrpcRevokeResponse> Revoke(GrpcRevokeRequest request, ServerCallContext context)
    {
        _logger.LogInformation("The method {InvokedMethod} was invoked with args: {@Request}",
            nameof(Revoke),
            request);
        
        var user = await _unitOfWork.IdentityUserRepository.GetManagedUserByNameAsync(request.UserName);
        if (user is null) return new GrpcRevokeResponse { Success = false };

        user.RefreshToken = null;
        await _unitOfWork.IdentityUserRepository.UpdateManagedUserAsync(user);

        return new GrpcRevokeResponse { Success = true };
    }

    public override async Task<GrpcRevokeAllResponse> RevokeAll(GrpcRevokeAllRequest request, ServerCallContext context)
    {
        _logger.LogInformation("The method {InvokedMethod} was invoked with args: {@Request}",
            nameof(RevokeAll),
            request);
        
        var users = await _unitOfWork.IdentityUserRepository.GetAllManagedUsersAsync();
        foreach (var user in users)
        {
            user.RefreshToken = null;
            await _unitOfWork.IdentityUserRepository.UpdateManagedUserAsync(user);
        }

        return new GrpcRevokeAllResponse { Success = true };
    }
}
