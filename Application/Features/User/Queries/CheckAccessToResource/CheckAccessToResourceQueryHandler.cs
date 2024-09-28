using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Application.Features.User.Queries.CheckAccessToResource;

public class CheckAccessToResourceQueryHandler(IAuthorizationService authorizationService) 
    : IRequestHandler<CheckAccessToResourceQuery, AuthorizationResult>
{
    public async Task<AuthorizationResult> Handle(CheckAccessToResourceQuery request, CancellationToken cancellationToken)
    {
        return await authorizationService.AuthorizeAsync(request.User, request.Resource, request.PolicyName);
    }
}