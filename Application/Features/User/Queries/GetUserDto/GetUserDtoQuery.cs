using Application.Dto;
using MediatR;

namespace Application.Features.User.Queries.GetUserDto;

public record GetUserDtoQuery(string Login) : IRequest<UserDto?>;
