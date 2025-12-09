using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class BasketItemRepository(ApplicationDbContext dbContext) : IBasketItemRepository
{
    public async Task<List<BasketItem>> GetBasketItems(string userName, CancellationToken cancellationToken = default)
    {
        var basketItems= dbContext.BasketItems.AsQueryable();
        return await basketItems
            .AsNoTracking()
            .OrderBy(bi => bi.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<BasketItem?> GetBasketItem(int basketItemId, CancellationToken cancellationToken = default)
    {
        var basketItems= dbContext.BasketItems.AsQueryable();
        return await basketItems.FirstOrDefaultAsync(bi => bi.Id == basketItemId, cancellationToken: cancellationToken);
    }

    public async Task<int> CreateBasketItem(BasketItem basketItem, CancellationToken cancellationToken = default)
    {
        await dbContext.BasketItems.AddAsync(basketItem, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return basketItem.Id;
    }

    public async Task UpdateBasketItem(BasketItem basketItem, CancellationToken cancellationToken = default)
    {
        dbContext.BasketItems.Update(basketItem);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteBasketItem(int basketItemId, CancellationToken cancellationToken = default)
    {
        var basketItems= dbContext.BasketItems.AsQueryable();
        var basketItem = await basketItems.FirstOrDefaultAsync(bi => bi.Id == basketItemId, cancellationToken: cancellationToken);

        if (basketItem != null)
        {
            dbContext.BasketItems.Remove(basketItem);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}