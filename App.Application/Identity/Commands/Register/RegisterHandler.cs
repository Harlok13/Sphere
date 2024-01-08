using App.Application.Identity.Commands.Authenticate;
using App.Application.Identity.Repositories;
using App.Application.Identity.Services;
using App.Application.Repositories;
using App.Application.Repositories.UnitOfWork;
using App.Contracts.Identity.Requests;
using App.Contracts.Identity.Responses;
using App.Domain.Identity.Entities;
using Mediator;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace App.Application.Identity.Commands.Register;

public class RegisterHandler : ICommandHandler<RegisterCommand, AuthenticateResponse>
{
    private readonly ILogger<RegisterHandler> _logger;
    private readonly IJwtService _jwtService;
    private readonly IPlayerRepository _playerRepository;
    private readonly IPlayerInfoRepository _playerInfoRepository;
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAppUnitOfWork _appUnitOfWork;
    private readonly IMediator _mediator;

    public RegisterHandler(
        ILogger<RegisterHandler> logger,
        IJwtService jwtService,
        IPlayerRepository playerRepository,
        IPlayerInfoRepository playerInfoRepository,
        IApplicationUserRepository applicationUserRepository,
        IAppUnitOfWork appUnitOfWork,
        UserManager<ApplicationUser> userManager,
        IMediator mediator)
    {
        _logger = logger;
        _jwtService = jwtService;
        _playerRepository = playerRepository;
        _playerInfoRepository = playerInfoRepository;
        _applicationUserRepository = applicationUserRepository;
        _appUnitOfWork = appUnitOfWork;
        _userManager = userManager;
        _mediator = mediator;
    }
    
    public async ValueTask<AuthenticateResponse> Handle(RegisterCommand command, CancellationToken cT)
    {
        var registerRequest = command.RegisterRequest;

        var user = new ApplicationUser
        {
            Email = registerRequest.Email,
            UserName = registerRequest.UserName
        };

        var result = await _applicationUserRepository.CreateManagedUserAsync(user, registerRequest.Password, cT);

        if (!result.Succeeded)
        {
            foreach(var err in result.Errors)
                _logger.LogError(err.ToString());
            throw new Exception();  // TODO: ex
            // return new(Guid.NewGuid(), "", "", "", "", new PlayerInfoResponse(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));  // TODO: ???
        }

        var findUser = await _applicationUserRepository.GetUserByEmailAsync(registerRequest.Email, cT);
        
        if (findUser is null)  throw new Exception($"User {registerRequest.Email} not found"); // TODO: custom ex

        await _applicationUserRepository.AddToRoleAsync(findUser, cT);
        await _playerInfoRepository.CreatePlayerInfoAsync(findUser.Id, findUser.UserName!, cT);
        // await _playerRepository.CreatePlayerAsync(findUser, cT);

        // await _appUnitOfWork.SaveChangesAsync(cT);
        var authCommand = new AuthenticateCommand(
            new AuthenticateRequest(registerRequest.Email, registerRequest.Password));

        return await _mediator.Send(authCommand, cT);
    }
}