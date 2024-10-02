using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace Application.Features.User.Commands.Logout;

public class LogoutCommandHandler(IHttpContextAccessor httpContextAccessor) : IRequestHandler<LogoutCommand>
{
    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var context = httpContextAccessor.HttpContext;
        if (context == null)
        {
            throw new NullReferenceException("HttpContext is null");
        }
        
        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}