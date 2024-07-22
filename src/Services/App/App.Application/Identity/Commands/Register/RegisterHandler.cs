// using App.Application.Identity.Commands.Authenticate;
// using App.Application.Identity.Repositories;
// using App.Application.Repositories.UnitOfWork;
// using App.Contracts.Identity.Requests;
// using App.Contracts.Identity.Responses;
// using App.Domain.Identity.Entities;
// using Mediator;
// using Microsoft.Extensions.Logging;
//
// namespace App.Application.Identity.Commands.Register;
//
// public class RegisterHandler : ICommandHandler<RegisterCommand, AuthenticateResponse>
// {
//     private readonly ILogger<RegisterHandler> _logger;
//     private readonly IApplicationUserRepository _applicationUserRepository;
//     private readonly IAppUnitOfWork _unitOfWork;
//     private readonly IMediator _mediator;
//
//     public RegisterHandler(
//         ILogger<RegisterHandler> logger,
//         IApplicationUserRepository applicationUserRepository,
//         IAppUnitOfWork unitOfWork,
//         IMediator mediator)
//     {
//         _logger = logger;
//         _applicationUserRepository = applicationUserRepository;
//         _unitOfWork = unitOfWork;
//         _mediator = mediator;
//     }
//     
//     public async ValueTask<AuthenticateResponse> Handle(RegisterCommand command, CancellationToken cT)
//     {
//         command.Deconstruct(out RegisterRequest request);
//
//         var user = new ApplicationUser
//         {
//             Email = request.Email,
//             UserName = request.UserName
//         };
//
//         var result = await _applicationUserRepository.CreateManagedUserAsync(user, request.Password, cT);
//         if (!result.Succeeded)
//         {
//             foreach(var err in result.Errors)
//                 _logger.LogError(err.Description);
//             throw new Exception();  // TODO: ex
//         }
//
//         var findUser = await _applicationUserRepository.GetUserByEmailAsync(request.Email, cT);
//         if (findUser is null)  throw new Exception($"User {request.Email} not found"); // TODO: custom ex
//
//         await _applicationUserRepository.AddToRoleAsync(findUser, cT);
//         await _unitOfWork.PlayerInfoRepository.CreatePlayerInfoAsync(findUser.Id, findUser.UserName!, cT);
//
//         var authCommand = new AuthenticateCommand(
//             new AuthenticateRequest(request.Email, request.Password));
//         return await _mediator.Send(authCommand, cT);
//     }
// }