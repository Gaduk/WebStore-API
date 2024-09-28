using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Application.Features.User.Queries.CheckAccessToResource;

public record CheckAccessToResourceQuery(
    ClaimsPrincipal User,
    object? Resource,
    string PolicyName) : IRequest<AuthorizationResult>;