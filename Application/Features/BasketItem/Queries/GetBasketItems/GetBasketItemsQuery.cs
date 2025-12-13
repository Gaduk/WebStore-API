using MediatR;

namespace Application.Features.BasketItem.Queries.GetBasketItems;

public record GetBasketItemsQuery(string UserName) : IRequest<List<Domain.Entities.BasketItem>>;