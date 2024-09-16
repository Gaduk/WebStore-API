using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class GoodRepository(ApplicationDbContext dbContext) : IGoodRepository
{
    public async Task<List<Good>> GetAllGoods(CancellationToken cancellationToken)
    {
        return await dbContext.Goods
            .AsNoTracking()
            .OrderBy(g => g.Id)
            .ToListAsync(cancellationToken);
    }
}