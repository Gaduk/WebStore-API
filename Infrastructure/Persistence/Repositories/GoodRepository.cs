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
}