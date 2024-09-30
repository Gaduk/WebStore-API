using Application.Dto.OrderedGoods;
using MediatR;

namespace Application.Features.Order.Commands.CreateOrder;

public record CreateOrderCommand(string UserName, IList<ShortOrderedGoodDto> OrderedGoods) : IRequest<int>;