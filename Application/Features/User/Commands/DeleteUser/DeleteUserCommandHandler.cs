using Domain.Repositories;
using MediatR;

namespace Application.Features.User.Commands.DeleteUser;

public class DeleteUserCommandHandler(IUserRepository userRepository) : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await userRepository.DeleteUser(request.User);
    }
}