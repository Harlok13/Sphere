using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Repositories;

public interface IIdentityUserRepository
{
    Task<User?> GetManagedUserByEmailAsync(string email, CancellationToken cT = default);

    Task<User?> GetUserByEmailAsync(string email, CancellationToken cT = default);

    Task<User?> GetManagedUserByNameAsync(string username, CancellationToken cT = default);

    ValueTask<bool> CheckManagedUserPasswordAsync(User user, string userPassword, CancellationToken cT = default);

    Task<IEnumerable<Guid>> GetRoleIdsAsync(User user, CancellationToken cT = default);

    Task<IEnumerable<IdentityRole<Guid>>> GetRoleNamesAsync(IEnumerable<Guid> roleIds, CancellationToken cT = default);

    Task<IdentityResult> CreateManagedUserAsync(User user, string userPassword, CancellationToken cT = default);

    Task AddToRoleAsync(User user, CancellationToken cT = default);

    Task UpdateManagedUserAsync(User user, CancellationToken cT = default);

    Task<IEnumerable<User>> GetAllManagedUsersAsync(CancellationToken cT = default);

    Task<User?> GetUserByIdAsync(Guid id, CancellationToken cT = default);
}