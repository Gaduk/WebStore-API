using Domain.Entities;

namespace Domain.Repositories;

public interface IBasketItemRepository
{
    Task<List<BasketItem>> GetBasketItems  (string userName      , CancellationToken cancellationToken = default);
    Task<BasketItem?>      GetBasketItem   (int basketItemId     , CancellationToken cancellationToken = default);
    Task<int>              CreateBasketItem(BasketItem basketItem, CancellationToken cancellationToken = default);
    Task                   UpdateBasketItem(BasketItem basketItem, CancellationToken cancellationToken = default);
    Task                   DeleteBasketItem(int basketItemId     , CancellationToken cancellationToken = default);
}