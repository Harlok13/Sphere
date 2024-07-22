using App.Domain.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace App.Application.Identity.Repositories;

public interface IApplicationUserRepository
{
    Task<ApplicationUser?> GetManagedUserByEmailAsync(string email, CancellationToken cT);

    Task<ApplicationUser?> GetUserByEmailAsync(string email, CancellationToken cT);

    Task<ApplicationUser?> GetManagedUserByNameAsync(string username, CancellationToken cT);

    ValueTask<bool> CheckManagedUserPasswordAsync(ApplicationUser user, string userPassword, CancellationToken cT);

    Task<IEnumerable<Guid>> GetRoleIdsAsync(ApplicationUser user, CancellationToken cT);

    Task<IEnumerable<IdentityRole<Guid>>> GetRoleNamesAsync(IEnumerable<Guid> roleIds, CancellationToken cT);

    Task<IdentityResult> CreateManagedUserAsync(ApplicationUser user, string userPassword, CancellationToken cT);

    Task AddToRoleAsync(ApplicationUser user, CancellationToken cT);

    Task UpdateManagedUserAsync(ApplicationUser user, CancellationToken cT);

    Task<IEnumerable<ApplicationUser>> GetAllManagedUsersAsync(CancellationToken cT);

    Task<ApplicationUser?> GetUserByIdAsync(Guid id, CancellationToken cT);
}