using System.Security.Claims;
using Application.Exceptions;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.User.Commands.UpdateUserRole;

public class UpdateUserRoleCommandHandler(
    IUserRepository userRepository, 
    UserManager<Domain.Entities.User> userManager) : IRequestHandler<UpdateUserRoleCommand>
{
    public async Task Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUser(request.UserName, cancellationToken: cancellationToken);
        if (user == null)
        {
            throw new NotFoundException($"User {request.UserName} is not found");
        }
        
        await userRepository.UpdateUserRole(user, request.IsAdmin, cancellationToken);
        
        var claims = new List<Claim>
        {
            new(ClaimTypes.Role, "admin")
        };
        switch (request.IsAdmin)
        {
            case true when !user.IsAdmin:
            {
                await userManager.AddClaimsAsync(user, claims);
                break;
            }
            case false when user.IsAdmin:
            {
                await userManager.RemoveClaimsAsync(user, claims);
                break;
            }
        }
    }
}