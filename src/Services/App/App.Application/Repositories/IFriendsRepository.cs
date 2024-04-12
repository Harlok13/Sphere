using App.Domain.Entities;

namespace App.Application.Repositories;

public interface IFriendsRepository
{
    Task AddToFriendsAsync(Friends friends, CancellationToken cT);
}