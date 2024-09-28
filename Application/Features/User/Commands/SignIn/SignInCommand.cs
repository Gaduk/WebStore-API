using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.User.Commands.SignIn;

public record SignInCommand(IList<Claim> Claims, HttpContext Context) : IRequest;