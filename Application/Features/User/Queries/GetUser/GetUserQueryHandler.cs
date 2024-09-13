using Domain.Dto.User;
using Domain.Repositories;
using MediatR;

namespace Application.Features.User.Queries.GetUser;

public class GetUserQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserQuery, UserDto?>
{
    public async Task<UserDto?> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        return await userRepository.GetUser(request.Login);
    }
}
