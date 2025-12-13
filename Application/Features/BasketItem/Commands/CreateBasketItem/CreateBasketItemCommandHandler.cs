using Application.Exceptions;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Application.Features.BasketItem.Commands.CreateBasketItem;

public class CreateBasketItemCommandHandler(
    IBasketItemRepository basketItemRepository,
    IUserRepository       userRepository,
    IGoodRepository       goodRepository,
    IAuthorizationService authorizationService,
    IHttpContextAccessor  httpContextAccessor) : IRequestHandler<CreateBasketItemCommand, Guid>
{
    public async Task<Guid> Handle(CreateBasketItemCommand request, CancellationToken cancellationToken)
    {   
        var context = httpContextAccessor.HttpContext;
        if (context == null)
        {
            throw new NullReferenceException("HttpContext is null");
        }
        
        var authorizationResult = await authorizationService.AuthorizeAsync(context.User, request.UserName, "HaveAccess");
        if (!authorizationResult.Succeeded)
        {
            throw new ForbiddenException();
        }

        var user = await userRepository.GetUser(request.UserName, includeOrders: false, cancellationToken: cancellationToken);
        if (user == null)
        {
            throw new NotFoundException($"User {request.UserName} is not found");
        }
        
        var good = await goodRepository.GetGood(request.GoodId, cancellationToken: cancellationToken);
        if (good == null)
        {
            throw new NotFoundException($"Good ID {request.GoodId} is not found");
        }
        
        var basketItem = new Domain.Entities.BasketItem
        {
            UserName = request.UserName, 
            GoodId   = request.GoodId,
            Amount   = request.Amount
        };
        
        return await basketItemRepository.CreateBasketItem(basketItem, cancellationToken);
    }
}