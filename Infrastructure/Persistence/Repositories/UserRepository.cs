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

    public async Task<List<User>> GetAllUsers(CancellationToken cancellationToken)
    {
        return await dbContext.Users
            .OrderBy(u => u.UserName)
            .ToListAsync(cancellationToken);
    }

    public async Task<User?> GetUser(string username, CancellationToken cancellationToken)
    {
        return await dbContext.Users
            .Where(u => u.UserName == username)
            .FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<User?> GetUserWithOrders(string username, CancellationToken cancellationToken)
    {
        return await dbContext.Users
            .Where(u => u.UserName == username)
            .Include(u => u.Orders)
            .FirstOrDefaultAsync(cancellationToken);
    }
}