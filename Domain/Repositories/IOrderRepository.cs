using Domain.Entities;

namespace Domain.Repositories;

public interface IOrderRepository
{
    Task<int> CreateOrder(Order order, CancellationToken cancellationToken = default);
    Task UpdateOrder(Order order, CancellationToken cancellationToken = default);
    Task<Order?> GetOrder(int orderId, bool includeOrderedGoods, CancellationToken cancellationToken = default);
    Task<List<Order>> GetOrders(CancellationToken cancellationToken = default);
}