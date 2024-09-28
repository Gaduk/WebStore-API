using Domain.Repositories;
using MediatR;

namespace Application.Features.User.Queries.GetUsers;

public class GetUsersQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUsersQuery, List<Domain.Entities.User>>
{
    public async Task<List<Domain.Entities.User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await userRepository.GetAllUsers(cancellationToken);
    }
}