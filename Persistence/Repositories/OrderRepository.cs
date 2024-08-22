using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class OrderRepository(ApplicationDbContext dbContext) : IOrderRepository
{
    public async Task CreateOrder(string login, OrderedGood[] orderedGoods)
    {
        var order = new Order
        {
            UserLogin = login,
            IsDone = false
        };
        await dbContext.Orders.AddAsync(order);
        await dbContext.SaveChangesAsync();
        int orderId = order.Id; 

        foreach (var orderedGood in orderedGoods)
        {
            if (orderedGood.Amount != 0)
            {
                orderedGood.OrderId = orderId;
                await dbContext.OrderedGoods.AddAsync(orderedGood);
            }
        }
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateOrderStatus(Order order, bool isDone)
    {
        order.IsDone = isDone;
        dbContext.Orders.Update(order);
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<Order>> GetAllOrders()
    {
        return await dbContext.Orders.ToListAsync();
    }

    public async Task<List<Order>> GetUserOrders(string login)
    {
        var orders = (from order in dbContext.Orders.AsParallel().AsOrdered()
            where order.UserLogin == login
            select order).ToList();
        return orders;
    }

    public async Task<Order?> GetOrder(int orderId)
    {
        var order = await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        return order;
    }
}