using Domain.Dto.User;
using Domain.Entities;

namespace Domain.Repositories;

public interface IUserRepository
{
    Task DeleteUser(User user, CancellationToken cancellationToken);
    Task UpdateUserRole(User user, bool isAdmin, CancellationToken cancellationToken);
    Task<List<UserDto>> GetAllUsers(CancellationToken cancellationToken);
    Task<UserDto?> GetUser(string login, CancellationToken cancellationToken);
}