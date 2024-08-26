using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Query;

namespace Web_API.Requirements.AccessRequirement.Handlers;

public class RequestLoginIsUserLoginHandler : AuthorizationHandler<AccessRequirement, string>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessRequirement requirement, string resource)
    {
        string? name = context.User.Identity?.Name;
        Console.WriteLine(name);
        Console.WriteLine(resource);

        if (context.User.Identity == null)
        {
            Console.WriteLine("something wrong");
        }

        if (name == resource)
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}