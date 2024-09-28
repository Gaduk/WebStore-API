using System.Security.Claims;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.User.Commands.UpdateUserRole;

public class UpdateUserRoleCommandHandler(IUserRepository userRepository, UserManager<Domain.Entities.User> userManager)
    : IRequestHandler<UpdateUserRoleCommand>
{
    public async Task Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        await userRepository.UpdateUserRole(request.User, request.IsAdmin, cancellationToken);
    }
}