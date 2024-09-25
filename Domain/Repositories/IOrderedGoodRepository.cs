using Domain.Entities;

namespace Domain.Repositories;

public interface IOrderedGoodRepository
{
    Task<List<OrderedGood>> GetAllOrderedGoods(CancellationToken cancellationToken);
    Task<List<OrderedGood>> GetOrderedGoods(int orderId, CancellationToken cancellationToken);
}