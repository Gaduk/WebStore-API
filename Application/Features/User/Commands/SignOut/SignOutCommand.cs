using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.User.Commands.SignOut;

public record SignOutCommand(HttpContext Context) : IRequest;