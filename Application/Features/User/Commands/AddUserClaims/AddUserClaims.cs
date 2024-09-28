using MediatR;

namespace Application.Features.User.Commands.AddUserClaims;

public record AddUserClaims(Domain.Entities.User User) : IRequest;