using MediatR;

namespace Application.Features.User.Queries.GetAllUsers;

public class GetAllUsersQuery : IRequest<List<Domain.Entities.User>>;
