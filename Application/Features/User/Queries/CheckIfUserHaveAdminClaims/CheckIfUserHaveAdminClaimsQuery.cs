using System.Security.Claims;
using MediatR;

namespace Application.Features.User.Queries.CheckIfUserHaveAdminClaims;

public record CheckIfUserHaveAdminClaimsQuery(ClaimsPrincipal User) : IRequest<bool>;