using Domain.Repositories;
using MediatR;

namespace Application.Features.User.Queries.GetAllUsers;

public class GetAllUsersQueryHandler(IUserRepository userRepository) : IRequestHandler<GetAllUsersQuery, List<Domain.Entities.User>>
{
    public async Task<List<Domain.Entities.User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        return await userRepository.GetAllUsers(cancellationToken);
    }
}