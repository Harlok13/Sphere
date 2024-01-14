using App.Application.Identity.Repositories;
using App.Domain.Identity.Entities;
using App.Domain.Identity.Enums;
using App.Infra.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace App.Infra.Identity.Repositories;

public class ApplicationUserRepository : IApplicationUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationContext _context;

    public ApplicationUserRepository(UserManager<ApplicationUser> userManager, ApplicationContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<ApplicationUser?> GetManagedUserByEmailAsync(string email, CancellationToken cT) =>
        await _userManager.FindByEmailAsync(email);

    public async Task<ApplicationUser?> GetUserByEmailAsync(string email, CancellationToken cT) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cT);

    public async Task<ApplicationUser?> GetManagedUserByNameAsync(string username, CancellationToken cT) =>
        await _userManager.FindByNameAsync(username);

    public async ValueTask<bool> CheckManagedUserPasswordAsync(
        ApplicationUser user,
        string userPassword,
        CancellationToken cT) =>
        await _userManager.CheckPasswordAsync(user, userPassword);

    public async Task<IEnumerable<Guid>> GetRoleIdsAsync(ApplicationUser user, CancellationToken cT) =>
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
        ApplicationUser user,
        string userPassword,
        CancellationToken cT) =>
        await _userManager.CreateAsync(user, userPassword);

    public async Task AddToRoleAsync(ApplicationUser user, CancellationToken cT) =>
        await _userManager.AddToRoleAsync(user, RoleConstants.Member);

    public async Task UpdateManagedUserAsync(ApplicationUser user, CancellationToken cT) =>
        await _userManager.UpdateAsync(user);

    public async Task<IEnumerable<ApplicationUser>> GetAllManagedUsersAsync(CancellationToken cT) =>
        await _userManager.Users.ToArrayAsync(cT);

    public async Task<ApplicationUser?> GetUserByIdAsync(Guid id, CancellationToken cT) =>
        await _context.Users.SingleOrDefaultAsync(e => e.Id == id, cT);
}