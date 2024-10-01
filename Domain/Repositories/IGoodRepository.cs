using Domain.Entities;

namespace Domain.Repositories;

public interface IGoodRepository
{
    Task<List<Good>> GetGoods(int? minPrice, int? maxPrice, CancellationToken cancellationToken = default);
}