using MediatR;

namespace Application.Features.User.Queries.GetUsers;

public class GetUsersQuery : IRequest<List<Domain.Entities.User>>;
