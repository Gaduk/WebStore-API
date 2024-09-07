using Application.Dto;
using Application.Dto.OrderedGoods;
using Domain.Entities;

namespace Application.Interfaces;

public interface IOrderRepository
{
    Task<int> CreateOrder(string login, CreateOrderedGoodDto[] orderedGoods);
    Task UpdateOrder(Order order, bool isDone);
    Task<List<Order>> GetAllOrders();
    Task<List<Order>> GetUserOrders(string login);
    Task<Order?> GetOrder(int orderId);
}