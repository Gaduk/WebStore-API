using MediatR;

namespace Application.Features.User.Commands.CreateUser;

public record CreateUserCommand(Domain.Entities.User User) : IRequest;