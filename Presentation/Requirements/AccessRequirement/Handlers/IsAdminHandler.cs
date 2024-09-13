using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Web_API.Requirements.AccessRequirement.Handlers;

public class IsAdminHandler : AuthorizationHandler<AccessRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessRequirement requirement)
    {
        var isAdmin = context.User.HasClaim(c => c is { Type: ClaimTypes.Role, Value: "admin" });
        if (isAdmin)
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}