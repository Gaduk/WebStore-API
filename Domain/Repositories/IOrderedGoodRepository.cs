using Domain.Dto.OrderedGoods;

namespace Domain.Repositories;

public interface IOrderedGoodRepository
{
    Task<List<OrderedGoodDto>> GetAllOrderedGoods(CancellationToken cancellationToken);
    Task<List<OrderedGoodDto>> GetOrderedGoods(int orderId, CancellationToken cancellationToken);
}