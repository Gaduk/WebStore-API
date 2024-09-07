using Application.Dto;
using Application.Dto.OrderedGoods;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class OrderRepository(ApplicationDbContext dbContext) : IOrderRepository
{
    public async Task<int> CreateOrder(string login, CreateOrderedGoodDto[] orderedGoods)
    {
        var order = new Order
        {
            UserName = login,
            IsDone = false
        };
        await dbContext.Orders.AddAsync(order);
        await dbContext.SaveChangesAsync();
        int orderId = order.Id; 

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

    public async Task UpdateOrder(Order order, bool isDone)
    {
        dbContext.Orders.Update(order);
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<Order>> GetAllOrders()
    {
        return await dbContext.Orders.ToListAsync();
    }

    public async Task<List<Order>> GetUserOrders(string login)
    {
        var orders = await dbContext
            .Orders
            .Where(o => o.UserName == login)
            .ToListAsync();

        return orders;
    }

    public async Task<Order?> GetOrder(int orderId)
    {
        var order = await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        return order;
    }
}