using Domain.Dto.User;
using MediatR;

namespace Application.Features.User.Queries.GetAllUsers;

public class GetAllUsersQuery : IRequest<List<UserDto>>;
