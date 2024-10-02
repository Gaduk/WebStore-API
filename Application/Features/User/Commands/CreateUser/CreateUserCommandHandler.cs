using System.Security.Claims;
using Application.Exceptions;
using Application.Services;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.User.Commands.CreateUser;

public class CreateUserCommandHandler(
    UserManager<Domain.Entities.User> userManager, 
    IUserRepository userRepository, 
    IMailService mailService) : IRequestHandler<CreateUserCommand>
{
    public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var userWithSameName = await userRepository.GetUser(request.UserName, cancellationToken: cancellationToken);
        if (userWithSameName != null)
        {
            throw new ConflictException($"User {request.UserName} already exist");
        }
        
        var user = new Domain.Entities.User
        {
            UserName = request.UserName,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email
        };
        
        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new BadRequestException($"Unable to create {user.UserName}");
        }
        
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Role, "user")
        };
        await userManager.AddClaimsAsync(user, claims);
        
        mailService.SubscribeToMailing(request.Email, request.UserName);
    }
}