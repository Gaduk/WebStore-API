using Domain.Dto.OrderedGoods;

namespace Domain.Repositories;

public interface IOrderedGoodRepository
{
    Task CreateOrderedGoods(int orderId, ShortOrderedGoodDto[] orderedGoods, CancellationToken cancellationToken);
    Task<List<OrderedGoodDto>> GetAllOrderedGoods(CancellationToken cancellationToken);
    Task<List<OrderedGoodDto>> GetOrderedGoods(int orderId, CancellationToken cancellationToken);
}