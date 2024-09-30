using Domain.Entities;

namespace Domain.Repositories;

public interface IOrderRepository
{
    Task<int> CreateOrder(Order order, CancellationToken cancellationToken);
    Task UpdateOrder(Order order, CancellationToken cancellationToken);
    Task<List<Order>> GetOrders(CancellationToken cancellationToken);
    Task<Order?> GetOrder(int orderId, CancellationToken cancellationToken);
}