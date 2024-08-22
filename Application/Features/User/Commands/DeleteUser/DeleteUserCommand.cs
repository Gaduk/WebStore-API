using MediatR;

namespace Application.Features.User.Commands.DeleteUser;

public record DeleteUserCommand(Domain.Entities.User User) : IRequest;