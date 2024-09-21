using Domain.Dto.Order;
using MediatR;

namespace Application.Features.Order.Queries.GetOrders;

public record GetOrdersQuery(string? Login) : IRequest<List<OrderDto>>;