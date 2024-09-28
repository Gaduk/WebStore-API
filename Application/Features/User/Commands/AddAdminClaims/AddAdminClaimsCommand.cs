using MediatR;

namespace Application.Features.User.Commands.AddAdminClaims;

public record AddAdminClaimsCommand(Domain.Entities.User User, bool IsAdmin) : IRequest;