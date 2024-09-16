using Domain.Dto.Order;
using Domain.Dto.OrderedGoods;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class OrderRepository(ApplicationDbContext dbContext) : IOrderRepository
{
    public async Task<int> CreateOrder(string userId, ShortOrderedGoodDto[] orderedGoods, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            UserId = userId,
            IsDone = false
        };
        await dbContext.Orders.AddAsync(order, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
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
            await dbContext.OrderedGoods.AddAsync(newOrderedGood, cancellationToken);
        }
        await dbContext.SaveChangesAsync(cancellationToken);
        return orderId;
    }

    public async Task UpdateOrder(Order order, CancellationToken cancellationToken)
    {
        dbContext.Orders.Update(order);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<OrderDto>> GetAllOrders(CancellationToken cancellationToken)
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
            .ToListAsync(cancellationToken);
    }

    public async Task<List<OrderDto>> GetUserOrders(string login, CancellationToken cancellationToken)
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
            .ToListAsync(cancellationToken);
    }

    public async Task<OrderDto?> GetOrder(int orderId, CancellationToken cancellationToken)
    {
        return await dbContext
            .Orders
            .Where(o => o.Id == orderId)
            .OrderBy(o => o.Id)
            .Select(o => new OrderDto 
            (
                o.Id, 
                o.User.UserName, 
                o.IsDone
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<Order?> GetOrderEntity(int orderId, CancellationToken cancellationToken)
    {
        return await dbContext
            .Orders
            .Where(o => o.Id == orderId)
            .FirstOrDefaultAsync(cancellationToken);
    }
}