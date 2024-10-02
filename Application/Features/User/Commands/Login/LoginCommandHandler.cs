using System.Security.Claims;
using Application.Exceptions;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.User.Commands.Login;

public class LoginCommandHandler(
    IUserRepository userRepository,
    UserManager<Domain.Entities.User> userManager,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<LoginCommand>
{
    public async Task Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUser(request.UserName, cancellationToken: cancellationToken);
        if (user == null)
        {
            throw new UnauthorizedException("Wrong username");
        }

        var checkPasswordTask = userManager.CheckPasswordAsync(user, request.Password);
        var getUserClaimsTask = userManager.GetClaimsAsync(user);
        await Task.WhenAll(checkPasswordTask, getUserClaimsTask);
        
        var passwordIsRight = checkPasswordTask.Result;
        var claims = getUserClaimsTask.Result;
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        
        if(!passwordIsRight) 
        {
            throw new UnauthorizedException("Wrong password");
        }

        var context = httpContextAccessor.HttpContext;
        if (context == null)
        {
            throw new NullReferenceException("HttpContext is null");
        }
        
        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
    }
}