using MediatR;

namespace Application.Features.BasketItem.Commands.UpsertBasketItem;

public record UpsertBasketItemCommand(string UserName, int GoodId, int Amount) : IRequest;