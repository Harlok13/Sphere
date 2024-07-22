using Identity.Application.Repositories;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using Identity.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repositories;

public class IdentityUserRepository : IIdentityUserRepository
{
    private readonly UserManager<User> _userManager;
    private readonly IdentityContext _context;

    public IdentityUserRepository(UserManager<User> userManager, IdentityContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<User?> GetManagedUserByEmailAsync(string email, CancellationToken cT) =>
        await _userManager.FindByEmailAsync(email);

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cT) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cT);

    public async Task<User?> GetManagedUserByNameAsync(string username, CancellationToken cT) =>
        await _userManager.FindByNameAsync(username);

    public async ValueTask<bool> CheckManagedUserPasswordAsync(
        User user,
        string userPassword,
        CancellationToken cT) =>
        await _userManager.CheckPasswordAsync(user, userPassword);

    public async Task<IEnumerable<Guid>> GetRoleIdsAsync(User user, CancellationToken cT) =>
        await _context.UserRoles
            .Where(r => r.UserId == user.Id)
            .Select(x => x.RoleId)
            .ToArrayAsync(cT);

    public async Task<IEnumerable<IdentityRole<Guid>>> GetRoleNamesAsync(IEnumerable<Guid> roleIds,
        CancellationToken cT) =>
        await _context.Roles
            .Where(x => roleIds.Contains(x.Id))
            .ToArrayAsync(cT);

    public async Task<IdentityResult> CreateManagedUserAsync(
        User user,
        string userPassword,
        CancellationToken cT) =>
        await _userManager.CreateAsync(user, userPassword);

    public async Task AddToRoleAsync(User user, CancellationToken cT) =>
        await _userManager.AddToRoleAsync(user, RoleConstants.Member);

    public async Task UpdateManagedUserAsync(User user, CancellationToken cT) =>
        await _userManager.UpdateAsync(user);

    public async Task<IEnumerable<User>> GetAllManagedUsersAsync(CancellationToken cT) =>
        await _userManager.Users.ToArrayAsync(cT);

    public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken cT) =>
        await _context.Users.SingleOrDefaultAsync(e => e.Id == id, cT);
}