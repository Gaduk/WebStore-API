using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.User.Commands.Logout;

public record LogoutCommand : IRequest;