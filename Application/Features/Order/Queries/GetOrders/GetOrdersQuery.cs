using MediatR;

namespace Application.Features.Order.Queries.GetOrders;

public record GetOrdersQuery : IRequest<List<Domain.Entities.Order>>;