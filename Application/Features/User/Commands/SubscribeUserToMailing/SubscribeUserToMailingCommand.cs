using MediatR;

namespace Application.Features.User.Commands.SubscribeUserToMailing;

public record SubscribeUserToMailingCommand(string Email, string UserName) : IRequest;