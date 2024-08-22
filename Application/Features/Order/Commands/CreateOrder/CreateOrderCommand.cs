using MediatR;

namespace Application.Features.Order.Commands.CreateOrder;

public record CreateOrderCommand(string Login, Domain.Entities.OrderedGood[] OrderedGoods) : IRequest;