using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Application.Features.User.Commands.SignOut;

public class SignOutCommandHandler : IRequestHandler<SignOutCommand>
{
    public async Task Handle(SignOutCommand request, CancellationToken cancellationToken)
    {
        await request.Context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}