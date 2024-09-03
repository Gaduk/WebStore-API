using Application.Dto;
using MediatR;

namespace Application.Features.User.Queries.GetAllUserDtos;

public class GetAllUserDtosQuery : IRequest<List<UserDto>>;
