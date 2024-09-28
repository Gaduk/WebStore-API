using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Application.Features.User.Commands.SignIn;

public class SignInCommandHandler : IRequestHandler<SignInCommand>
{
    public async Task Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var claimsIdentity = new ClaimsIdentity(request.Claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await request.Context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
    }
}