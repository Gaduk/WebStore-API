using Domain.Dto.Order;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class OrderRepository(ApplicationDbContext dbContext) : IOrderRepository
{
    public async Task<int> CreateOrder(string username, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            UserName = username,
            IsDone = false
        };
        await dbContext.Orders.AddAsync(order, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return order.Id;
    }

    public async Task UpdateOrder(Order order, CancellationToken cancellationToken)
    {
        dbContext.Orders.Update(order);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<OrderDto>> GetOrders(string? login, CancellationToken cancellationToken)
    {
        var orders = dbContext.Orders.AsQueryable();
        if (login != null)
        {
            orders = orders.Where(o => o.User.UserName == login);
        }
        return await orders
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