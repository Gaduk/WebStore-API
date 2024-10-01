using Domain.Entities;

namespace Domain.Repositories;

public interface IOrderRepository
{
    Task<int> CreateOrder(Order order, CancellationToken cancellationToken);
    Task UpdateOrder(Order order, CancellationToken cancellationToken);
    Task<Order?> GetOrder(int orderId, bool includeOrderedGoods, CancellationToken cancellationToken);
    Task<List<Order>> GetOrders(CancellationToken cancellationToken);
}