using Application.Interfaces;
using MediatR;

namespace Application.Features.User.Commands.CreateUser;

public class CreateUserCommandHandler(IUserRepository userRepository) : IRequestHandler<CreateUserCommand>
{
    public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        await userRepository.CreateUser(request.User);
    }
}
