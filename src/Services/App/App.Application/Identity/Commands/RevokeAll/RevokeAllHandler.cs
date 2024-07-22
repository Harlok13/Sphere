// using App.Application.Identity.Commands.Revoke;
// using App.Application.Identity.Repositories;
// using App.Application.Identity.Services;
// using App.Application.Repositories;
// using App.Application.Repositories.UnitOfWork;
// using App.Domain.Identity.Entities;
// using Mediator;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.Extensions.Logging;
//
// namespace App.Application.Identity.Commands.RevokeAll;
//
// public class RevokeAllHandler : ICommandHandler<RevokeAllCommand, bool>
// {
//     private readonly ILogger<RevokeAllHandler> _logger;
//     private readonly IApplicationUserRepository _applicationUserRepository;
//
//     public RevokeAllHandler(
//         ILogger<RevokeAllHandler> logger,
//         IApplicationUserRepository applicationUserRepository)
//     {
//         _logger = logger;
//         _applicationUserRepository = applicationUserRepository;
//     }
//     
//     public async ValueTask<bool> Handle(RevokeAllCommand command, CancellationToken cT)
//     {
//         var users = await _applicationUserRepository.GetAllManagedUsersAsync(cT);
//         foreach (var user in users)
//         {
//             user.RefreshToken = null;
//             await _applicationUserRepository.UpdateManagedUserAsync(user, cT);
//         }
//
//         return true;
//     }
// }