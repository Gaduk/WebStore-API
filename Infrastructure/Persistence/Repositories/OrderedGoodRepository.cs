using Dapper;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class OrderedGoodRepository(ApplicationDbContext dbContext) : IOrderedGoodRepository
{
    public async Task<List<OrderedGood>> GetOrderedGoods(int? minPrice, int? maxPrice, CancellationToken cancellationToken)
    {
        var connection = dbContext.Database.GetDbConnection();
        
        var sql = """
                       SELECT og."OrderId", og."GoodId", og."Amount", g."Id", g."Name", g."Price"
                       FROM "OrderedGoods" og
                       INNER JOIN "Goods" g ON og."GoodId" = g."Id"
                       WHERE 1 = 1
                  """;
        if (minPrice.HasValue)
            sql += """ AND g."Price" >= @MinPrice """;
        if (maxPrice.HasValue)
            sql += """ AND g."Price" <= @MaxPrice """;
        sql += """ ORDER BY og."Id" """;
        
        var parameters = new DynamicParameters();
        parameters.Add("@MinPrice", minPrice);
        parameters.Add("@MaxPrice", maxPrice);
        
        var command = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);
        var result = await connection.QueryAsync<OrderedGood, Good, OrderedGood>(
            command, 
            (og, g) =>
            {
                og.Good = g;
                return og;
            });
        return result.ToList();
    }
}