using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class GoodRepository(ApplicationDbContext dbContext) : IGoodRepository
{
    public async Task<List<Good>> GetAllGoods()
    {
        return await dbContext.Goods.ToListAsync();
    }
}