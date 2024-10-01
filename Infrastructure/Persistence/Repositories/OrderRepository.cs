using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class OrderRepository(ApplicationDbContext dbContext) : IOrderRepository
{
    public async Task<int> CreateOrder(Order order, CancellationToken cancellationToken = default)
    {
        await dbContext.Orders.AddAsync(order, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return order.Id;
    }

    public async Task UpdateOrder(Order order, CancellationToken cancellationToken = default)
    {
        dbContext.Orders.Update(order);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Order>> GetOrders(CancellationToken cancellationToken = default)
    {
        var orders = dbContext.Orders.AsQueryable();
        return await orders
            .AsNoTracking()
            .OrderBy(o => o.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<Order?> GetOrder(int orderId, bool includeOrderedGoods = false, CancellationToken cancellationToken = default)
    {
        var orders = dbContext.Orders.AsQueryable();
        if (includeOrderedGoods)
        {
            orders = orders
                .Include(o => o.OrderedGoods)
                .ThenInclude(og => og.Good);
        }
        var order = await orders
            .Where(o => o.Id == orderId)
            .OrderBy(o => o.Id)
            .SingleAsync(cancellationToken);
        return order;
    }
}