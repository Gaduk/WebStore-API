using Domain.Dto.Order;
using MediatR;

namespace Application.Features.Order.Queries.GetAllOrders;

public record GetAllOrdersQuery : IRequest<List<OrderDto>>;