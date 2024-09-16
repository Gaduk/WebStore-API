using Domain.Entities;

namespace Domain.Repositories;

public interface IGoodRepository
{
    Task<List<Good>> GetAllGoods(CancellationToken cancellationToken);
}