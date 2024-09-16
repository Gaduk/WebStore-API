using Domain.Repositories;
using MediatR;

namespace Application.Features.User.Commands.UpdateUserRole;

public class UpdateUserRoleCommandHandler(IUserRepository userRepository) : IRequestHandler<UpdateUserRoleCommand>
{
    public async Task Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        await userRepository.UpdateUserRole(request.User, request.IsAdmin, cancellationToken);
    }
}