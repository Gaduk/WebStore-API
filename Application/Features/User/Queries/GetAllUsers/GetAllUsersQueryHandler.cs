using Domain.Dto.User;
using Domain.Repositories;
using MediatR;

namespace Application.Features.User.Queries.GetAllUsers;

public class GetAllUsersQueryHandler(IUserRepository userRepository) : IRequestHandler<GetAllUsersQuery, List<UserDto>>
{
    public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        return await userRepository.GetAllUsers();
    }
}