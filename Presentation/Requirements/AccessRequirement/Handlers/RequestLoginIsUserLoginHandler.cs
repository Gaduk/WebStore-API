using Microsoft.AspNetCore.Authorization;

namespace Presentation.Requirements.AccessRequirement.Handlers;

public class RequestLoginIsUserLoginHandler : AuthorizationHandler<AccessRequirement, string>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessRequirement requirement, string resource)
    {
        var name = context.User.Identity?.Name;
        if (name == resource)
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}