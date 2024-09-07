using Application.Dto.OrderedGoods;
using MediatR;

namespace Application.Features.Order.Commands.CreateOrder;

public record CreateOrderCommand(string Login, CreateOrderedGoodDto[] OrderedGoods) : IRequest<int>;