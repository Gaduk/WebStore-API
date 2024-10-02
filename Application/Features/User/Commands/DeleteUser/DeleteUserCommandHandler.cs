using Application.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.Features.User.Commands.DeleteUser;

public class DeleteUserCommandHandler(IUserRepository userRepository) : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUser(request.UserName, cancellationToken: cancellationToken);
        if (user == null)
        {
            throw new NotFoundException($"User {request.UserName} is not found");
        }
        await userRepository.DeleteUser(user, cancellationToken);
    }
}