using App.Application.Extensions;
using App.Application.Identity.Repositories;
using App.Application.Identity.Services;
using App.Application.Repositories.UnitOfWork;
using App.Contracts.Identity.Requests;
using App.Contracts.Identity.Responses;
using App.Contracts.Mapper;
using App.Domain.Entities.PlayerInfoEntity;
using App.GrpcClient.IdentityClient;
using Mediator;
using Microsoft.Extensions.Logging;

namespace App.Application.Identity.Commands.Authenticate;

public class AuthenticateHandler : ICommandHandler<AuthenticateCommand, AuthenticateResponse>
{
    private readonly ILogger<AuthenticateHandler> _logger;
    private readonly IJwtService _jwtService;
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly IAppUnitOfWork _unitOfWork;
    // private readonly IIdentityClient _identityClient;

    public AuthenticateHandler(
        ILogger<AuthenticateHandler> logger,
        IJwtService jwtService,
        IApplicationUserRepository applicationUserRepository, 
        IAppUnitOfWork unitOfWork)
        // IIdentityClient identityClient)
    {
        _logger = logger;
        _jwtService = jwtService;
        _applicationUserRepository = applicationUserRepository;
        _unitOfWork = unitOfWork;
        // _identityClient = identityClient;
    }

    public async ValueTask<AuthenticateResponse> Handle(AuthenticateCommand command, CancellationToken cT)
    {
        command.Deconstruct(out AuthenticateRequest request);

        // var resp = await _identityClient.AuthenticateAsync(new IdentityClient.AuthenticateRequest
        //     { Email = request.Email, Password = request.Password });
        
        // _logger.LogCritical(resp.Email);
        
        _logger.LogDebug($"Auth request\nEmail: {request.Email}\nPassword: {request.Password}");

        var managedUser = await _applicationUserRepository.GetManagedUserByEmailAsync(request.Email, cT);

        if (managedUser == null)
        {
            // return BadRequest("Bad credentials");
            throw new Exception();  // TODO: ex
        }

        var isPasswordValid = await _applicationUserRepository.CheckManagedUserPasswordAsync(managedUser, request.Password, cT);

        if (!isPasswordValid)
        {
            // return BadRequest("Bad credentials");
            _logger.LogDebug(isPasswordValid.ToString(), "password");
            throw new Exception();  // TODO: ex
        }

        var user = await _applicationUserRepository.GetUserByEmailAsync(request.Email, cT);

        if (user is null)
        {
            // return Unathorized();
            throw new Exception();  // TODO: ex
        }

        var roleIds = await _applicationUserRepository.GetRoleIdsAsync(user, cT);
        var roles = await _applicationUserRepository.GetRoleNamesAsync(roleIds, cT);

        var accessToken = _jwtService.GetJwtToken(user, roles);
        user.RefreshToken = _jwtService.GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtService.RefreshTokenValidityInDays);

        await _unitOfWork.SaveChangesAsync(cT);
        var playerInfoResult = await _unitOfWork.PlayerInfoRepository.GetPlayerInfoByIdAsync(user.Id, cT);
        if (!playerInfoResult.TryFromResult(out PlayerInfo? playerInfo, out var playerInfoErrors))
        {
            // TODO: finish
        }
        var statisticResponse = PlayerMapper.MapPlayerInfoToPlayerInfoResponse(playerInfo!);

        return new AuthenticateResponse(
            PlayerId: user.Id,
            PlayerName: user.UserName,
            Email: user.Email,
            Token: accessToken,
            RefreshToken: user.RefreshToken,
            PlayerInfo: statisticResponse);
    }
}