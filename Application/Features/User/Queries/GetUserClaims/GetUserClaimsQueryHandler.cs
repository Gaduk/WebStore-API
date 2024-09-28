using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.User.Queries.GetUserClaims;

public class GetUserClaimsQueryHandler(UserManager<Domain.Entities.User> userManager) 
    : IRequestHandler<GetUserClaimsQuery, IList<Claim>>
{
    public async Task<IList<Claim>> Handle(GetUserClaimsQuery request, CancellationToken cancellationToken)
    {
        return await userManager.GetClaimsAsync(request.User);
    }
}