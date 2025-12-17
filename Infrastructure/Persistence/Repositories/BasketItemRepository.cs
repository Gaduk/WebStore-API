using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class BasketItemRepository(ApplicationDbContext dbContext) : IBasketItemRepository
{
    public async Task CreateBasketItem(BasketItem basketItem, CancellationToken cancellationToken = default)
    {
        await dbContext.BasketItems.AddAsync(basketItem, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task UpdateBasketItem(BasketItem basketItem, CancellationToken cancellationToken = default)
    {
        dbContext.BasketItems.Update(basketItem);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteBasketItem(string userName, int goodId, CancellationToken cancellationToken = default)
    {
        var basketItem = await GetBasketItem(userName, goodId, cancellationToken);

        if (basketItem != null)
        {
            dbContext.BasketItems.Remove(basketItem);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
    
    public async Task<List<BasketItem>> GetBasketItems(string userName, CancellationToken cancellationToken = default)
    {
        var basketItems= dbContext.BasketItems.AsQueryable();
        return await basketItems
            .AsNoTracking()
            .Where(bi => bi.UserName == userName)
            .OrderBy(bi => bi.GoodId)
            .ToListAsync(cancellationToken);
    }

    public async Task<BasketItem?> GetBasketItem(string userName, int goodId, CancellationToken cancellationToken = default)
    {
        var basketItems= dbContext.BasketItems.AsQueryable();
        
        return await basketItems.FirstOrDefaultAsync(
            bi => bi.UserName == userName && 
                  bi.GoodId   == goodId, 
            cancellationToken: cancellationToken);
    }
}