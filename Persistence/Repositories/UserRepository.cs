using Application.Dto.User;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class UserRepository(ApplicationDbContext dbContext) : IUserRepository
{
    public async Task CreateUser(User user)
    {
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteUser(User user)
    {
        dbContext.Users.Remove(user);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateUserRole(User user, bool isAdmin)
    {
        user.IsAdmin = isAdmin;
        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<UserDto>> GetAllUsers()
    {
        return await dbContext.Users
            .OrderBy(u => u.UserName)
            .Select(user => new UserDto(
                user.UserName,
                user.FirstName,
                user.LastName,
                user.PhoneNumber,
                user.Email,
                user.IsAdmin
            )).ToListAsync();
    }

    public async Task<UserDto?> GetUser(string login)
    {
        return await dbContext.Users
            .Where(u => u.UserName == login)
            .Select(user => new UserDto(
                user.UserName, 
                user.FirstName,
                user.LastName,
                user.PhoneNumber,
                user.Email,
                user.IsAdmin
            ))
            .FirstOrDefaultAsync();
    }
}