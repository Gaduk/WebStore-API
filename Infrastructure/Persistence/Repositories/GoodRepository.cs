using Dapper;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class GoodRepository(ApplicationDbContext dbContext) : IGoodRepository
{
    public async Task<List<Good>> GetGoods(int? minPrice, int? maxPrice, CancellationToken cancellationToken = default)
    {
        var connection = dbContext.Database.GetDbConnection();
        
        var sql = """ SELECT * FROM "Goods" WHERE TRUE """;
        var parameters = new DynamicParameters();

        if (minPrice.HasValue)
        {
            sql += """ AND "Goods"."Price" >= @MinPrice """;
            parameters.Add("@MinPrice", minPrice);
        }

        if (maxPrice.HasValue)
        {
            sql += """ AND "Goods"."Price" <= @MaxPrice """;
            parameters.Add("@MaxPrice", maxPrice);
        }
        sql += """ ORDER BY "Goods"."Id" """;
        
        var command = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);
        var result = await connection.QueryAsync<Good>(command);
        return result.ToList();
    }

    public async Task<Good?> GetGood(int goodId, CancellationToken cancellationToken = default)
    {
        var goods = dbContext.Goods.AsQueryable();
        var good = await goods.FirstOrDefaultAsync(g => g.Id == goodId, cancellationToken: cancellationToken);
        
        return good;
    }

    public async Task<int> CreateGood(Good good, CancellationToken cancellationToken = default)
    {
        await dbContext.Goods.AddAsync(good, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return good.Id;
    }

    public async Task UpdateGood(Good good, CancellationToken cancellationToken = default)
    {
        dbContext.Goods.Update(good);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteGood(int goodId, CancellationToken cancellationToken = default)
    {
        var goods = dbContext.Goods.AsQueryable();
        var good = await goods.FirstOrDefaultAsync(g => g.Id == goodId, cancellationToken: cancellationToken);

        if (good != null)
        {
            dbContext.Goods.Remove(good);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}