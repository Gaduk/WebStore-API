using Application.Features.OrderedGood;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class OrderedGoodRepository(ApplicationDbContext dbContext) : IOrderedGoodRepository
{
    public async Task<List<OrderedGoodDto>> GetAllOrderedGoodDtos()
    {
        var orderedGoodsDto = await dbContext.OrderedGoods
            .Join(dbContext.Goods,
                og => og.GoodId,
                g => g.Id,
                (og, g) => new OrderedGoodDto(og.OrderId, og.GoodId, og.Amount, g.Name, g.Price)
            ).ToListAsync();
            
        return orderedGoodsDto;
    }

    public async Task<List<OrderedGoodDto>> GetOrderedGoodDtos(int orderId)
    {
        var allOrderedGoodsDto = await GetAllOrderedGoodDtos();
        var orderedGoodsDto = 
            (from og in allOrderedGoodsDto
                where og.OrderId == orderId
                select og).ToList();
            
        return orderedGoodsDto;
    }
}