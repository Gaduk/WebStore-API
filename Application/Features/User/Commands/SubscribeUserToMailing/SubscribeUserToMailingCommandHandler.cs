using Application.Services;
using MediatR;

namespace Application.Features.User.Commands.SubscribeUserToMailing;

public class SubscribeUserToMailingCommandHandler(IMailService mailService) : IRequestHandler<SubscribeUserToMailingCommand>
{
    public Task Handle(SubscribeUserToMailingCommand request, CancellationToken cancellationToken)
    {
        mailService.SubscribeToMailing(request.Email, request.Username);
        return Task.CompletedTask;
    }
}