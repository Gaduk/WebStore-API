using Application.Dto;
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
        /*
        var orders = (from order in dbContext.Orders.AsParallel().AsOrdered()
            where order.UserName == user.UserName
            select order).ToList();
            
        var orderIds = orders.Select(o => o.Id).ToList(); 
            
        var orderedGoods = (from orderedGood in dbContext.OrderedGoods.AsParallel().AsOrdered()
            where orderIds.Contains(orderedGood.OrderId)
            select orderedGood).ToList();
            
        dbContext.OrderedGoods.RemoveRange(orderedGoods);
        dbContext.Orders.RemoveRange(orders);
        dbContext.Users.Remove(user);
        await dbContext.SaveChangesAsync();
        */
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