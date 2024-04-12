using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Repositories;

public interface IApplicationUserRepository
{
    Task<ApplicationUser?> GetManagedUserByEmailAsync(string email, CancellationToken cT = default);

    Task<ApplicationUser?> GetUserByEmailAsync(string email, CancellationToken cT = default);

    Task<ApplicationUser?> GetManagedUserByNameAsync(string username, CancellationToken cT = default);

    ValueTask<bool> CheckManagedUserPasswordAsync(ApplicationUser user, string userPassword, CancellationToken cT = default);

    Task<IEnumerable<Guid>> GetRoleIdsAsync(ApplicationUser user, CancellationToken cT = default);

    Task<IEnumerable<IdentityRole<Guid>>> GetRoleNamesAsync(IEnumerable<Guid> roleIds, CancellationToken cT = default);

    Task<IdentityResult> CreateManagedUserAsync(ApplicationUser user, string userPassword, CancellationToken cT = default);

    Task AddToRoleAsync(ApplicationUser user, CancellationToken cT = default);

    Task UpdateManagedUserAsync(ApplicationUser user, CancellationToken cT = default);

    Task<IEnumerable<ApplicationUser>> GetAllManagedUsersAsync(CancellationToken cT = default);

    Task<ApplicationUser?> GetUserByIdAsync(Guid id, CancellationToken cT = default);
}