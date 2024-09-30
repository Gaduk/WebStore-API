using Domain.Entities;

namespace Domain.Repositories;

public interface IOrderedGoodRepository
{
    Task<List<OrderedGood>> GetOrderedGoods(int? minPrice, int? maxPrice, CancellationToken cancellationToken);
}