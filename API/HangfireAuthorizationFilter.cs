using System.Security.AccessControl;
using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace Web_API;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        return true;
    }
}