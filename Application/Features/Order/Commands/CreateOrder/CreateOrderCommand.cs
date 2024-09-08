using Application.Dto.OrderedGoods;
using MediatR;

namespace Application.Features.Order.Commands.CreateOrder;

public record CreateOrderCommand(string userId, CreateOrderedGoodDto[] OrderedGoods) : IRequest<int>;