using Dapper;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class OrderedGoodRepository(ApplicationDbContext dbContext) : IOrderedGoodRepository
{
    public async Task<List<OrderedGood>> GetOrderedGoods(CancellationToken cancellationToken)
    {
        var connection = dbContext.Database.GetDbConnection();
        
        const string sql = """
                              SELECT og."OrderId", og."GoodId", og."Amount", g."Id", g."Name", g."Price"
                              FROM "OrderedGoods" og
                              INNER JOIN "Goods" g ON og."GoodId" = g."Id"
                              ORDER BY og."Id"
                           """;
        
        var command = new CommandDefinition(sql, cancellationToken: cancellationToken);
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