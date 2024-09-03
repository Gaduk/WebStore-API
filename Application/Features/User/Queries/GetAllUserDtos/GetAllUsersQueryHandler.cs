using Application.Dto;
using Application.Interfaces;
using MediatR;

namespace Application.Features.User.Queries.GetAllUserDtos;

public class GetAllUsersQueryHandler(IUserRepository userRepository) : IRequestHandler<GetAllUserDtosQuery, List<UserDto>>
{
    public async Task<List<UserDto>> Handle(GetAllUserDtosQuery request, CancellationToken cancellationToken)
    {
        return await userRepository.GetAllUserDtos();
    }
}