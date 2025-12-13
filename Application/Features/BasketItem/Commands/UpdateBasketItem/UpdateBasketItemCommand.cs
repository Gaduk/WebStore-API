using MediatR;

namespace Application.Features.BasketItem.Commands.UpdateBasketItem;

public record UpdateBasketItemCommand(Guid BasketItemId, int Amount) : IRequest;