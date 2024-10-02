using MediatR;

namespace Application.Features.User.Commands.CreateUser;

public record CreateUserCommand(
    string UserName,
    string Password,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Email) : IRequest;