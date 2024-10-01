using Domain.Entities;

namespace Domain.Repositories;

public interface IOrderedGoodRepository
{
    Task<List<OrderedGood>> GetOrderedGoods(CancellationToken cancellationToken);
}