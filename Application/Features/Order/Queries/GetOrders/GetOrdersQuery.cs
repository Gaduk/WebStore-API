using Application.Dto.Order;
using MediatR;

namespace Application.Features.Order.Queries.GetOrders;

public record GetOrdersQuery(string? UserName) : IRequest<List<OrderDto>>;