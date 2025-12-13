using Application.Exceptions;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Application.Features.BasketItem.Commands.UpdateBasketItem;

public class UpdateBasketItemCommandHandler(
    IBasketItemRepository basketItemRepository,  
    IUserRepository       userRepository,
    IAuthorizationService authorizationService,
    IHttpContextAccessor  httpContextAccessor) : IRequestHandler<UpdateBasketItemCommand>
{
    public async Task Handle(UpdateBasketItemCommand request, CancellationToken cancellationToken)
    {
        var basketItem = await basketItemRepository.GetBasketItem(request.BasketItemId, cancellationToken);
        if (basketItem == null)
        {
            throw new NotFoundException($"BasketItem ID {request.BasketItemId} is not found");
        }
        
        var user = await userRepository.GetUser(basketItem.UserName, includeOrders: false, cancellationToken: cancellationToken);
        if (user == null)
        {
            throw new NotFoundException($"User {basketItem.UserName} is not found");
        }
        
        var context = httpContextAccessor.HttpContext;
        if (context == null)
        {
            throw new NullReferenceException("HttpContext is null");
        }
        
        var authorizationResult = await authorizationService.AuthorizeAsync(context.User, basketItem.UserName, "HaveAccess");
        if (!authorizationResult.Succeeded)
        {
            throw new ForbiddenException();
        }
        
        basketItem.Amount = request.Amount;
        
        await basketItemRepository.UpdateBasketItem(basketItem, cancellationToken);
    }
}