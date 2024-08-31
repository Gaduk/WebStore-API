using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Web_API.Requirements.AccessRequirement.Handlers;

public class IsAdminHandler : AuthorizationHandler<AccessRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessRequirement requirement)
    {
        bool isAdmin = context.User.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "admin");
        if (isAdmin)
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}