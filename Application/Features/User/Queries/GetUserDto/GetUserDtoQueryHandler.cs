using Application.Dto;
using Application.Interfaces;
using MediatR;

namespace Application.Features.User.Queries.GetUserDto;

public class GetUserDtoQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserDtoQuery, UserDto?>
{
    public async Task<UserDto?> Handle(GetUserDtoQuery request, CancellationToken cancellationToken)
    {
        return await userRepository.GetUserDto(request.Login);
    }
}
