using Domain.Dto.Order;
using Domain.Dto.OrderedGoods;
using Domain.Entities;

namespace Domain.Repositories;

public interface IOrderRepository
{
    Task<int> CreateOrder(string userId, ShortOrderedGoodDto[] orderedGoods, CancellationToken cancellationToken);
    Task UpdateOrder(Order order, CancellationToken cancellationToken);
    Task<List<OrderDto>> GetAllOrders(CancellationToken cancellationToken);
    Task<List<OrderDto>> GetUserOrders(string login, CancellationToken cancellationToken);
    Task<OrderDto?> GetOrder(int orderId, CancellationToken cancellationToken);
    Task<Order?> GetOrderEntity(int orderId, CancellationToken cancellationToken);
}