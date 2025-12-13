using MediatR;

namespace Application.Features.BasketItem.Commands.CreateBasketItem;

public record CreateBasketItemCommand(string UserName, int GoodId, int Amount) : IRequest<Guid>;