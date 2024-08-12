using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Web_API;

public static class RightsChecker
{
    public static bool IsCookiesLogin(HttpContext context, string login)
    {
        string? cookiesLogin = context.User.FindFirst(ClaimTypes.Name)?.Value;
        if (string.IsNullOrEmpty(cookiesLogin)) return false;
        return login == cookiesLogin;
    }

    public static bool IsAdmin(HttpContext context)
    {
        string? cookiesRole = context.User.FindFirst(ClaimTypes.Role)?.Value;
        if (string.IsNullOrEmpty(cookiesRole)) return false;
        return cookiesRole == "admin";
    }

    public static async Task<bool> IsLoginAsync(ApplicationContext db, string login)
    {
        User? user = await db.Users.FirstOrDefaultAsync(u => u.Login == login);
        return user != null;
    }
}