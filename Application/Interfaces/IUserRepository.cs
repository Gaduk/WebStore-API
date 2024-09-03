using Application.Dto;
using Domain.Entities;

namespace Application.Interfaces;

public interface IUserRepository
{
    Task CreateUser(User user);
    Task DeleteUser(User user);
    Task UpdateUserRole(User user, bool isAdmin);
    Task<List<UserDto>> GetAllUserDtos();
    Task<UserDto?> GetUserDto(string login);
}