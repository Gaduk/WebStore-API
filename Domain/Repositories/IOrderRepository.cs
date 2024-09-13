using Domain.Dto.Order;
using Domain.Dto.OrderedGoods;
using Domain.Entities;

namespace Domain.Repositories;

public interface IOrderRepository
{
    Task<int> CreateOrder(string userId, ShortOrderedGoodDto[] orderedGoods);
    Task UpdateOrder(Order order);
    Task<List<OrderDto>> GetAllOrders();
    Task<List<OrderDto>> GetUserOrders(string login);
    Task<OrderDto?> GetOrder(int orderId);
    Task<Order?> GetOrderEntity(int orderId);
}