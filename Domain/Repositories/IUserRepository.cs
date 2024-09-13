using Domain.Dto.User;
using Domain.Entities;

namespace Domain.Repositories;

public interface IUserRepository
{
    Task DeleteUser(User user);
    Task UpdateUserRole(User user, bool isAdmin);
    Task<List<UserDto>> GetAllUsers();
    Task<UserDto?> GetUser(string login);
}