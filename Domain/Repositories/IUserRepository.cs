using Domain.Entities;

namespace Domain.Repositories;

public interface IUserRepository
{
    Task DeleteUser(User user, CancellationToken cancellationToken);
    Task UpdateUserRole(User user, bool isAdmin, CancellationToken cancellationToken);
    Task<List<User>> GetAllUsers(CancellationToken cancellationToken);
    Task<User?> GetUser(string username, CancellationToken cancellationToken);
    Task<User?> GetUserWithOrders(string username, CancellationToken cancellationToken);
}