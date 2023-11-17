using Microsoft.AspNetCore.Identity;
using Sphere.DAL.Models;

namespace Sphere.DAL.Auth;

public interface IAuthDbApi
{
    public Task<ApplicationUser?> GetManagedUserByEmailAsync(string email);

    public Task<ApplicationUser?> GetUserByEmailAsync(string email);

    public Task<ApplicationUser?> GetManagedUserByNameAsync(string username);

    public Task<bool> CheckManagedUserPasswordAsync(ApplicationUser user, string userPassword);

    public Task<IEnumerable<int>> GetRoleIdsAsync(ApplicationUser user);

    public Task<IEnumerable<IdentityRole<int>>> GetRoleNamesAsync(IEnumerable<int> roleIds);

    public Task SaveChangesAsync();

    public Task<IdentityResult> CreateManagedUserAsync(ApplicationUser user, string userPassword);

    public Task AddToRoleAsync(ApplicationUser user);

    public Task UpdateManagedUserAsync(ApplicationUser user);

    public Task<IEnumerable<ApplicationUser>> GetAllManagedUsersAsync();
}