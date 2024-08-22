using MediatR;

namespace Application.Features.User.Queries.GetUserByLogin;

public record GetUserByLoginQuery(string Login) : IRequest<Domain.Entities.User?>;
