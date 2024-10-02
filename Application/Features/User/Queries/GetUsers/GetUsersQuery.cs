using Application.Dto.User;
using MediatR;

namespace Application.Features.User.Queries.GetUsers;

public class GetUsersQuery : IRequest<List<UserDto>>;
