using Application.Dto.User;
using Application.Exceptions;
using AutoMapper;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Application.Features.User.Queries.GetUser;

public class GetUserQueryHandler(
    IUserRepository userRepository, 
    IAuthorizationService authorizationService, 
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetUserQuery, UserDto?>
{
    public async Task<UserDto?> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var context = httpContextAccessor.HttpContext;
        if (context == null)
        {
            throw new NullReferenceException("HttpContext is null");
        }
        
        var authorizationResult = await authorizationService.AuthorizeAsync(context.User, request.UserName, "HaveAccess");
        if (!authorizationResult.Succeeded)
        {
            throw new ForbiddenException();
        }

        var user = await userRepository.GetUser(request.UserName, cancellationToken: cancellationToken);
        if(user == null)
        {
            throw new NotFoundException($"User {request.UserName} is not found");
        }

        return mapper.Map<UserDto>(user);
    }
}
