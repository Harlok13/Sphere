// using Identity.Application.Commands.Authenticate;
// using Identity.Application.Repositories.UnitOfWork;
// using Identity.Contracts.Requests;
// using Identity.Contracts.Responses;
// using Identity.Domain.Entities;
// using Mediator;
// using Microsoft.Extensions.Logging;
//
// namespace Identity.Application.Commands.Register;
//
// public class RegisterHandler : ICommandHandler<RegisterCommand, AuthenticateResponse>
// {
//     private readonly ILogger<RegisterHandler> _logger;
//     private readonly IIdentityUnitOfWork _unitOfWork;
//     private readonly IMediator _mediator;
//
//     public RegisterHandler(
//         ILogger<RegisterHandler> logger,
//         IIdentityUnitOfWork unitOfWork,
//         IMediator mediator)
//     {
//         _logger = logger;
//         _unitOfWork = unitOfWork;
//         _mediator = mediator;
//     }
//     
//     public async ValueTask<AuthenticateResponse> Handle(RegisterCommand command, CancellationToken cT)
//     {
//         command.Deconstruct(out RegisterRequest request);
//
//         var user = new User
//         {
//             Email = request.Email,
//             UserName = request.UserName
//         };
//
//         var result = await _unitOfWork.IdentityUserRepository.CreateManagedUserAsync(user, request.Password, cT);
//         if (!result.Succeeded)
//         {
//             foreach(var err in result.Errors)
//                 _logger.LogError(err.Description);
//             throw new Exception();  // TODO: ex
//         }
//
//         var findUser = await _unitOfWork.IdentityUserRepository.GetUserByEmailAsync(request.Email, cT);
//         if (findUser is null)  throw new Exception($"User {request.Email} not found"); // TODO: custom ex
//
//         await _unitOfWork.IdentityUserRepository.AddToRoleAsync(findUser, cT);
//         await _unitOfWork.PlayerInfoRepository.CreatePlayerInfoAsync(findUser.Id, findUser.UserName!, cT);
//
//         var authCommand = new AuthenticateCommand(
//             new AuthenticateRequest(request.Email, request.Password));
//         return await _mediator.Send(authCommand, cT);
//     }
// }