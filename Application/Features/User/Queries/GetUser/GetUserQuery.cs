using Application.Dto.User;
using MediatR;

namespace Application.Features.User.Queries.GetUser;

public record GetUserQuery(string UserName) : IRequest<UserDto?>;
