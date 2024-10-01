using Domain.Entities;

namespace Domain.Repositories;

public interface IUserRepository
{
    Task DeleteUser(User user, CancellationToken cancellationToken = default);
    Task UpdateUserRole(User user, bool isAdmin, CancellationToken cancellationToken = default);
    Task<List<User>> GetAllUsers(CancellationToken cancellationToken = default);
    Task<User?> GetUser(string username, bool includeOrders = false, CancellationToken cancellationToken = default);
}