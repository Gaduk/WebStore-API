using Domain.Dto.User;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository(ApplicationDbContext dbContext) : IUserRepository
{
    public async Task DeleteUser(User user, CancellationToken cancellationToken)
    {
        dbContext.Users.Remove(user);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateUserRole(User user, bool isAdmin, CancellationToken cancellationToken)
    {
        user.IsAdmin = isAdmin;
        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<UserDto>> GetAllUsers(CancellationToken cancellationToken)
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
            )).ToListAsync(cancellationToken);
    }

    public async Task<UserDto?> GetUser(string login, CancellationToken cancellationToken)
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
            .FirstOrDefaultAsync(cancellationToken);
    }
}