using Domain.Dto.Order;
using Domain.Dto.OrderedGoods;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class OrderRepository(ApplicationDbContext dbContext) : IOrderRepository
{
    public async Task<int> CreateOrder(string userId, ShortOrderedGoodDto[] orderedGoods)
    {
        var order = new Order
        {
            UserId = userId,
            IsDone = false
        };
        await dbContext.Orders.AddAsync(order);
        await dbContext.SaveChangesAsync();
        var orderId = order.Id; 

        foreach (var orderedGood in orderedGoods)
        {
            if (orderedGood.Amount == 0) continue;
            var newOrderedGood = new OrderedGood
            {
                OrderId = orderId,
                GoodId = orderedGood.GoodId,
                Amount = orderedGood.Amount
            }; 
            await dbContext.OrderedGoods.AddAsync(newOrderedGood);
        }
        await dbContext.SaveChangesAsync();
        return orderId;
    }

    public async Task UpdateOrder(Order order)
    {
        dbContext.Orders.Update(order);
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<OrderDto>> GetAllOrders()
    {
        return await dbContext
            .Orders
            .OrderBy(o => o.Id)
            .Select(o => new OrderDto
            (
                o.Id, 
                o.User.UserName, 
                o.IsDone
            ))
            .ToListAsync();
    }

    public async Task<List<OrderDto>> GetUserOrders(string login)
    {
        return await dbContext
            .Orders
            .Where(o => o.User.UserName == login)
            .OrderBy(o => o.Id)
            .Select(o => new OrderDto 
            (
                o.Id, 
                o.User.UserName, 
                o.IsDone
            ))
            .ToListAsync();
    }

    public async Task<OrderDto?> GetOrder(int orderId)
    {
        return await dbContext
            .Orders
            .Include(o => o.User)
            .Where(o => o.Id == orderId)
            .OrderBy(o => o.Id)
            .Select(o => new OrderDto 
            (
                o.Id, 
                o.User.UserName, 
                o.IsDone
            ))
            .FirstOrDefaultAsync();
    }
    
    public async Task<Order?> GetOrderEntity(int orderId)
    {
        return await dbContext
            .Orders
            .Where(o => o.Id == orderId)
            .FirstOrDefaultAsync();
    }
}