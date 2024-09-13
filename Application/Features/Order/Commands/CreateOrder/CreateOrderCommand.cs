using Domain.Dto.OrderedGoods;
using MediatR;

namespace Application.Features.Order.Commands.CreateOrder;

public record CreateOrderCommand(string UserId, CreateOrderedGoodDto[] OrderedGoods) : IRequest<int>;