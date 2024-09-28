using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.User.Commands.AddUserClaims;

public class AddUserClaimsHandler(UserManager<Domain.Entities.User> userManager) : IRequestHandler<AddUserClaims>
{
    public async Task Handle(AddUserClaims request, CancellationToken cancellationToken)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, request.User.UserName ?? ""),
            new(ClaimTypes.Role, "user")
        };
        await userManager.AddClaimsAsync(request.User, claims);
    }
}