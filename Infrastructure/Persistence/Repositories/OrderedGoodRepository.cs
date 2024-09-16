using Dapper;
using Domain.Dto.OrderedGoods;
using Domain.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class OrderedGoodRepository(ApplicationDbContext dbContext) : IOrderedGoodRepository
{
    public async Task<List<OrderedGoodDto>> GetAllOrderedGoods(CancellationToken cancellationToken)
    {
        return await dbContext
            .OrderedGoods
            .OrderBy(og => og.Id)
            .Select(og => new OrderedGoodDto(
                og.OrderId,
                og.GoodId,
                og.Amount,
                og.Good.Name,
                og.Good.Price))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<OrderedGoodDto>> GetOrderedGoods(int orderId)
    {
        var connection = dbContext.Database.GetDbConnection();
        
        const string query = """
                             SELECT og."OrderId", og."GoodId", og."Amount", g."Name", g."Price"
                             FROM "OrderedGoods" og
                             INNER JOIN "Goods" g ON og."GoodId" = g."Id"
                             WHERE og."OrderId" = @OrderId
                             ORDER BY og."Id"
                             """;
        var parameters = new { OrderId = orderId };
        var result = await connection.QueryAsync<OrderedGoodDto>(query, parameters);
        return result.ToList();
    }
}