using Application.Dto.Order;
using MediatR;

namespace Application.Features.Order.Queries.GetOrder;

public record GetOrderQuery(int OrderId) : IRequest<OrderDto?>;