using MediatR;

namespace Application.Features.User.Commands.UpdateUserRole;

public record UpdateUserRoleCommand(string UserName, bool IsAdmin) : IRequest;