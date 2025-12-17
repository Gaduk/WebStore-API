using Domain.Entities;

namespace Domain.Repositories;

public interface IBasketItemRepository
{
    Task<List<BasketItem>> GetBasketItems  (string userName,             CancellationToken cancellationToken = default);
    Task<BasketItem?>      GetBasketItem   (string userName, int goodId, CancellationToken cancellationToken = default);
    Task                   UpsertBasketItem(BasketItem basketItem,       CancellationToken cancellationToken = default);
    Task                   DeleteBasketItem(string userName, int goodId, CancellationToken cancellationToken = default);
}