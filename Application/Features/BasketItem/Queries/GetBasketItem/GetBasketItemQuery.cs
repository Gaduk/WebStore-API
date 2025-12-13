using MediatR;

namespace Application.Features.BasketItem.Queries.GetBasketItem;

public record GetBasketItemQuery(Guid BasketItemId) : IRequest<Domain.Entities.BasketItem?>;