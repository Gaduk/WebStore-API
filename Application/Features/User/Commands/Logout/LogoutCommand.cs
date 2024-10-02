using MediatR;

namespace Application.Features.User.Commands.Logout;

public record LogoutCommand : IRequest;