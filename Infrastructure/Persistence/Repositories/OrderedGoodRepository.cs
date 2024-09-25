using Dapper;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class OrderedGoodRepository(ApplicationDbContext dbContext) : IOrderedGoodRepository
{
    public async Task<List<OrderedGood>> GetAllOrderedGoods(CancellationToken cancellationToken)
    {
        return await dbContext
            .OrderedGoods
            .AsNoTracking()
            .Include(og => og.Good)
            .OrderBy(og => og.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<OrderedGood>> GetOrderedGoods(int orderId, CancellationToken cancellationToken)
    {
        var connection = dbContext.Database.GetDbConnection();
        const string query = """
                             SELECT og."OrderId", og."GoodId", og."Amount", g."Id", g."Name", g."Price"
                             FROM "OrderedGoods" og
                             INNER JOIN "Goods" g ON og."GoodId" = g."Id"
                             WHERE og."OrderId" = @OrderId
                             ORDER BY og."Id"
                             """;
        var parameters = new { OrderId = orderId };
        var command = new CommandDefinition(query, parameters, cancellationToken: cancellationToken);
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