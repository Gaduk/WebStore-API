using Domain.Dto.Order;
using Domain.Dto.OrderedGoods;
using Domain.Entities;

namespace Domain.Repositories;

public interface IOrderRepository
{
    Task<int> CreateOrder(string userId, CreateOrderedGoodDto[] orderedGoods);
    Task UpdateOrder(Order order, bool isDone);
    Task<List<OrderDto>> GetAllOrders();
    Task<List<OrderDto>> GetUserOrders(string login);
    Task<OrderDto?> GetOrder(int orderId);
    Task<Order?> GetOrderEntity(int orderId);
}