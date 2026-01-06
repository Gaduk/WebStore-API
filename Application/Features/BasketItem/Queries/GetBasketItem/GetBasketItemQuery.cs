using Application.Dto.BasketItem;
using MediatR;

namespace Application.Features.BasketItem.Queries.GetBasketItem;

public record GetBasketItemQuery(string UserName, int GoodId) : IRequest<BasketItemDto?>;