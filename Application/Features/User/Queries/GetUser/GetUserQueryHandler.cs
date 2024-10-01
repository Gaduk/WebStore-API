using Domain.Repositories;
using MediatR;

namespace Application.Features.User.Queries.GetUser;

public class GetUserQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserQuery, Domain.Entities.User?>
{
    public async Task<Domain.Entities.User?> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        return await userRepository.GetUser(request.UserName, request.IncludeOrders, cancellationToken);
    }
}
