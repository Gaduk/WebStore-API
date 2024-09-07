using Application.Dto;
using Application.Dto.OrderedGoods;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class OrderedGoodRepository(ApplicationDbContext dbContext) : IOrderedGoodRepository
{
    public async Task<List<OrderedGoodDto>> GetAllOrderedGoods()
    {
        return await dbContext
            .OrderedGoods
            .Include(og => og.Good)
            .Select(og => new OrderedGoodDto(
                og.OrderId,
                og.GoodId,
                og.Amount,
                og.Good.Name,
                og.Good.Price))
            .ToListAsync();
    }

    public async Task<List<OrderedGoodDto>> GetOrderedGoods(int orderId)
    {
        return await dbContext
            .OrderedGoods
            .Include(og => og.Good)
            .Select(og => new OrderedGoodDto(
                og.OrderId,
                og.GoodId,
                og.Amount,
                og.Good.Name,
                og.Good.Price))
            .Where(og => og.OrderId == orderId)
            .ToListAsync();
    }
}