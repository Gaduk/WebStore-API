using System.Security.Claims;
using MediatR;

namespace Application.Features.User.Queries.CheckIfUserHaveAdminClaims;

public class CheckIfUserHaveAdminClaimsQueryHandler : IRequestHandler<CheckIfUserHaveAdminClaimsQuery, bool>
{
    public Task<bool> Handle(CheckIfUserHaveAdminClaimsQuery request, CancellationToken cancellationToken)
    {
        var hasAdminClaim = request.User.HasClaim(ClaimTypes.Role, "admin");
        return Task.FromResult(hasAdminClaim);
    }
}