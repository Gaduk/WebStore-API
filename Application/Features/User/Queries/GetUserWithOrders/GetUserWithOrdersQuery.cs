using MediatR;

namespace Application.Features.User.Queries.GetUserWithOrders;

public record GetUserWithOrdersQuery(string UserName) : IRequest<Domain.Entities.User?>;