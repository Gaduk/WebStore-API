using MediatR;

namespace Application.Features.BasketItem.Commands.DeleteBasketItem;

public record DeleteBasketItemCommand(Guid BasketItemId) : IRequest;