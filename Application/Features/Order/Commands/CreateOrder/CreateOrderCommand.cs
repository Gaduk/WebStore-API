using Domain.Dto.OrderedGoods;
using MediatR;

namespace Application.Features.Order.Commands.CreateOrder;

public record CreateOrderCommand(string UserId, ShortOrderedGoodDto[] OrderedGoods) : IRequest<int>;