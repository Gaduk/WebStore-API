using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class GoodRepository(ApplicationDbContext dbContext) : IGoodRepository
{
    public async Task<List<Good>> GetAllGoods()
    {
        return await dbContext.Goods
            .AsNoTracking()
            .OrderBy(g => g.Id)
            .ToListAsync();
    }
}