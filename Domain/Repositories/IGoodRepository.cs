using Domain.Entities;

namespace Domain.Repositories;

public interface IGoodRepository
{
    Task<List<Good>> GetGoods(int? minPrice, int? maxPrice, CancellationToken cancellationToken = default);
    Task<Good?> GetGood(int goodId, CancellationToken cancellationToken = default);
    Task<int> CreateGood(Good good, CancellationToken cancellationToken = default);
    Task UpdateGood(Good good, CancellationToken cancellationToken = default);
    Task DeleteGood(int goodId, CancellationToken cancellationToken = default);
}