using Domain.Entities;

namespace Application.Interfaces;

public interface IOrderRepository
{
    Task CreateOrder(string login, OrderedGood[] orderedGoods);
    Task UpdateOrderStatus(Order order, bool isDone);
    Task<List<Order>> GetAllOrders();
    Task<List<Order>> GetUserOrders(string login);
    Task<Order?> GetOrder(int orderId);
}