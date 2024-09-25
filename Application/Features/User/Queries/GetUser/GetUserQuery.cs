using MediatR;

namespace Application.Features.User.Queries.GetUser;

public record GetUserQuery(string Login) : IRequest<Domain.Entities.User?>;
