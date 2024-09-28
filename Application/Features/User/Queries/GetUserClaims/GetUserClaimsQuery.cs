using System.Security.Claims;
using MediatR;

namespace Application.Features.User.Queries.GetUserClaims;

public record GetUserClaimsQuery(Domain.Entities.User User) : IRequest<IList<Claim>>;