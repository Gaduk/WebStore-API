using MediatR;

namespace Application.Features.User.Commands.UpdateUserRole;

public record UpdateUserRoleCommand(Domain.Entities.User User, bool IsAdmin) : IRequest;