using Application.Features.User;
using Domain.Entities;

namespace Application.Interfaces;

public interface IUserRepository
{
    Task CreateUser(User user);
    Task DeleteUser(User user);
    Task<List<User>> GetAllUsers();
    Task<User?> GetUser(string login);
}