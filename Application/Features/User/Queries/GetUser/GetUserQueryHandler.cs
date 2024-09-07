using Application.Dto;
using Application.Dto.User;
using Application.Interfaces;
using MediatR;

namespace Application.Features.User.Queries.GetUser;

public class GetUserQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserQuery, UserDto?>
{
    public async Task<UserDto?> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        return await userRepository.GetUser(request.Login);
    }
}
