using MediatR;

namespace Application.Features.Order.Queries.GetUserOrders;

public record GetUserOrdersQuery(string Login) : IRequest<List<Domain.Entities.Order>>;