using Application.Dto.Order;
using MediatR;

namespace Application.Features.Order.Queries.GetUserOrders;

public record GetUserOrdersQuery(string Login) : IRequest<List<OrderDto>>;