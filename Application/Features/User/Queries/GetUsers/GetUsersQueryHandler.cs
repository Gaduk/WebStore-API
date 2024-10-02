using Application.Dto.User;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.Features.User.Queries.GetUsers;

public class GetUsersQueryHandler(
    IUserRepository userRepository,
    IMapper mapper) : IRequestHandler<GetUsersQuery, List<UserDto>>
{
    public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAllUsers(cancellationToken);
        return mapper.Map<List<UserDto>>(users);
    }
}