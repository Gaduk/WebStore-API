using MediatR;

namespace Application.Features.User.Commands.Login;

public record LoginCommand(string UserName, string Password) : IRequest;