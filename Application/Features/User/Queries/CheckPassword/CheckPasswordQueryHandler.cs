using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.User.Queries.CheckPassword;

public class CheckPasswordQueryHandler(UserManager<Domain.Entities.User> userManager)
    : IRequestHandler<CheckPasswordQuery, bool>
{
    public Task<bool> Handle(CheckPasswordQuery request, CancellationToken cancellationToken)
    {
        return userManager.CheckPasswordAsync(request.User, request.Password);
    }
}