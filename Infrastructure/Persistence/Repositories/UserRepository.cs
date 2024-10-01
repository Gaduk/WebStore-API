using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository(ApplicationDbContext dbContext) : IUserRepository
{
    public async Task DeleteUser(User user, CancellationToken cancellationToken = default)
    {
        dbContext.Users.Remove(user);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateUserRole(User user, bool isAdmin, CancellationToken cancellationToken = default)
    {
        user.IsAdmin = isAdmin;
        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<User>> GetAllUsers(CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .AsNoTracking()
            .OrderBy(u => u.UserName)
            .ToListAsync(cancellationToken);
    }

    public async Task<User?> GetUser(
        string username, 
        bool includeOrders = false, 
        CancellationToken cancellationToken = default)
    {
        var users = dbContext.Users.AsQueryable();
        if (includeOrders)
        {
            users = users
                .Include(u => u.Orders);
        }
        var user = await users
            .Where(u => u.UserName == username)
            .SingleAsync(cancellationToken);
        return user;
    }

}