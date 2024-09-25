using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class OrderRepository(ApplicationDbContext dbContext) : IOrderRepository
{
    public async Task<int> CreateOrder(Order order, CancellationToken cancellationToken)
    {
        await dbContext.Orders.AddAsync(order, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return order.Id;
    }

    public async Task UpdateOrder(Order order, CancellationToken cancellationToken)
    {
        dbContext.Orders.Update(order);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Order>> GetOrders(string? login, CancellationToken cancellationToken)
    {
        var orders = dbContext.Orders.AsQueryable();
        if (login != null)
        {
            orders = orders.Where(o => o.User.UserName == login);
        }
        return await orders
            .AsNoTracking()
            .Include(o => o.User)
            .OrderBy(o => o.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<Order?> GetOrder(int orderId, CancellationToken cancellationToken)
    {
        return await dbContext
            .Orders
            .AsNoTracking()
            .Include(o => o.User)
            .Include(o => o.OrderedGoods)
            .ThenInclude(og => og.Good)
            .Where(o => o.Id == orderId)
            .OrderBy(o => o.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }
}