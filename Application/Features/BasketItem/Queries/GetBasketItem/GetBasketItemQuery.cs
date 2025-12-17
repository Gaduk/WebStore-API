using MediatR;

namespace Application.Features.BasketItem.Queries.GetBasketItem;

public record GetBasketItemQuery(string UserName, int GoodId) : IRequest<Domain.Entities.BasketItem?>;