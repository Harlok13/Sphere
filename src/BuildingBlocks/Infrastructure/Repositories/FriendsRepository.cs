using App.Application.Repositories;
using App.Domain.Entities;
using Infrastructure.Data.Context;

namespace Infrastructure.Repositories;

public class FriendsRepository : IFriendsRepository
{
    private readonly ApplicationContext _context;

    public FriendsRepository(ApplicationContext context) => _context = context;

    public async Task AddToFriendsAsync(Friends friends, CancellationToken cT)
    {
        await _context.Set<Friends>().AddAsync(friends, cT);
    }
}