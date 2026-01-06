using Application.Dto.BasketItem;
using MediatR;

namespace Application.Features.BasketItem.Queries.GetBasketItems;

public record GetBasketItemsQuery(string UserName) : IRequest<List<BasketItemDto>>;