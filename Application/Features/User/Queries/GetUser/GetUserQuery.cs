using MediatR;

namespace Application.Features.User.Queries.GetUser;

public record GetUserQuery(string UserName, bool IncludeOrders = false) : IRequest<Domain.Entities.User?>;
