using MediatR;

namespace Application.Features.Order.Queries.GetOrders;

public record GetOrdersQuery(string? Login) : IRequest<List<Domain.Entities.Order>>;