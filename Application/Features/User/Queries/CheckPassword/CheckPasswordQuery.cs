using MediatR;

namespace Application.Features.User.Queries.CheckPassword;

public record CheckPasswordQuery(Domain.Entities.User User, string Password) : IRequest<bool>;