using MediatR;

namespace Application.Features.BasketItem.Commands.DeleteBasketItem;

public record DeleteBasketItemCommand(string UserName, int GoodId) : IRequest;