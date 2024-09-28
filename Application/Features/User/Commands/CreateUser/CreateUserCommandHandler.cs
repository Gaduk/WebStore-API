using System.Security.Claims;
using Application.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.User.Commands.CreateUser;

public class CreateUserCommandHandler(UserManager<Domain.Entities.User> userManager, IMailService mailService) 
    : IRequestHandler<CreateUserCommand, (IdentityResult, Domain.Entities.User)>
{
    public async Task<(IdentityResult, Domain.Entities.User)> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new Domain.Entities.User
        {
            UserName = request.UserName,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email
        };
        
        var result = await userManager.CreateAsync(user, request.Password);
        return (result, user);
    }
}