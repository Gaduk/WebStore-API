using Application.Interfaces;
using MediatR;

namespace Application.Features.User.Queries.GetUserByLogin;

public class GetUserByLoginQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserByLoginQuery, Domain.Entities.User?>
{
    public async Task<Domain.Entities.User?> Handle(GetUserByLoginQuery request, CancellationToken cancellationToken)
    {
        return await userRepository.GetUser(request.Login);
    }
}
