using Application.Dto;
using Application.Dto.User;
using Domain.Entities;

namespace Application.Interfaces;

public interface IUserRepository
{
    Task CreateUser(User user);
    Task DeleteUser(User user);
    Task UpdateUserRole(User user, bool isAdmin);
    Task<List<UserDto>> GetAllUsers();
    Task<UserDto?> GetUser(string login);
}