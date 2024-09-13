using Microsoft.AspNetCore.Authorization;
namespace Web_API.Requirements.AccessRequirement.Handlers;

public class RequestLoginIsUserLoginHandler : AuthorizationHandler<AccessRequirement, string>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessRequirement requirement, string resource)
    {
        string? name = context.User.Identity?.Name;
        if (name == resource)
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}