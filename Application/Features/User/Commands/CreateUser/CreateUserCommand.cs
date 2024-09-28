using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.User.Commands.CreateUser;

public record CreateUserCommand(
    string UserName = "",
    string Password = "",
    string FirstName = "",
    string LastName = "",
    string PhoneNumber = "",
    string Email = "") : IRequest<(IdentityResult, Domain.Entities.User)>;