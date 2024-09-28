using MediatR;

namespace Application.Features.User.Queries.GetUser;

public record GetUserQuery(string UserName) : IRequest<Domain.Entities.User?>;
