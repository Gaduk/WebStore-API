using Domain.Dto.OrderedGoods;
using MediatR;

namespace Application.Features.Order.Commands.CreateOrder;

public record CreateOrderCommand(string UserName, List<ShortOrderedGoodDto> OrderedGoods) : IRequest<int>;