using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.User.Commands.AddAdminClaims;

public class AddAdminClaimsCommandHandler(UserManager<Domain.Entities.User> userManager) 
    : IRequestHandler<AddAdminClaimsCommand>
{
    public async Task Handle(AddAdminClaimsCommand request, CancellationToken cancellationToken)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Role, "admin")
        };
        switch (request.IsAdmin)
        {
            case true when !request.User.IsAdmin:
            {
                await userManager.AddClaimsAsync(request.User, claims);
                break;
            }
            case false when request.User.IsAdmin:
            {
                await userManager.RemoveClaimsAsync(request.User, claims);
                break;
            }
        }
    }
}