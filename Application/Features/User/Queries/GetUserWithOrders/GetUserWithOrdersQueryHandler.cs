using Domain.Repositories;
using MediatR;

namespace Application.Features.User.Queries.GetUserWithOrders;

public class GetUserWithOrdersQueryHandler(IUserRepository userRepository) 
    : IRequestHandler<GetUserWithOrdersQuery, Domain.Entities.User?>
{
    public async Task<Domain.Entities.User?> Handle(GetUserWithOrdersQuery request, CancellationToken cancellationToken)
    {
        return await userRepository.GetUserWithOrders(request.UserName, cancellationToken);
    }
}