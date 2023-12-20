using App.Application.Identity.Repositories;
using App.Application.Identity.Services;
using App.Application.Mapper;
using App.Application.Repositories;
using App.Application.Repositories.UnitOfWork;
using App.Contracts.Identity.Responses;
using App.Domain.Entities;
using App.Domain.Identity.Entities;
using Mediator;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace App.Application.Identity.Commands.Authenticate;

public class AuthenticateHandler : ICommandHandler<AuthenticateCommand, AuthenticateResponse>
{
    private readonly ILogger<AuthenticateHandler> _logger;
    private readonly IJwtService _jwtService;
    private readonly IPlayerRepository _playerRepository;
    private readonly IPlayerInfoRepository _playerInfoRepository;
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAppUnitOfWork _appUnitOfWork;

    public AuthenticateHandler(
        ILogger<AuthenticateHandler> logger,
        IJwtService jwtService,
        IPlayerRepository playerRepository,
        IPlayerInfoRepository playerInfoRepository,
        IApplicationUserRepository applicationUserRepository,
        IAppUnitOfWork appUnitOfWork,
        UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _jwtService = jwtService;
        _playerRepository = playerRepository;
        _playerInfoRepository = playerInfoRepository;
        _applicationUserRepository = applicationUserRepository;
        _appUnitOfWork = appUnitOfWork;
        _userManager = userManager;
    }

    public async ValueTask<AuthenticateResponse> Handle(AuthenticateCommand command, CancellationToken cT)
    {
        var authRequest = command.AuthenticateRequest;
        
        _logger.LogInformation($"Auth request\nEmail: {authRequest.Email}\nPassword: {authRequest.Password}");

        var managedUser = await _applicationUserRepository.GetManagedUserByEmailAsync(authRequest.Email, cT);

        if (managedUser == null)
        {
            // return BadRequest("Bad credentials");
            throw new Exception();  // TODO: ex
        }

        var isPasswordValid = await _applicationUserRepository.CheckManagedUserPasswordAsync(managedUser, authRequest.Password, cT);

        if (!isPasswordValid)
        {
            // return BadRequest("Bad credentials");
            _logger.LogInformation(isPasswordValid.ToString(), "password");
            throw new Exception();  // TODO: ex
        }

        var user = await _applicationUserRepository.GetUserByEmailAsync(authRequest.Email, cT);

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

        await _appUnitOfWork.SaveChangesAsync(cT);
        var statistic = await _playerInfoRepository.GetPlayerInfoByIdAsync(user.Id, cT);
        var statisticResponse = PlayerMapper.MapPlayerInfoToPlayerInfoResponse(statistic);

        return new AuthenticateResponse(
            PlayerId: user.Id,
            PlayerName: user.UserName,
            Email: user.Email,
            Token: accessToken,
            RefreshToken: user.RefreshToken,
            PlayerInfo: statisticResponse);
    }
}