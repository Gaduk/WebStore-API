using Application.Exceptions;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Application.Features.BasketItem.Commands.DeleteBasketItem;

public class DeleteBasketItemCommandHandler(
    IBasketItemRepository basketItemRepository,
    IUserRepository       userRepository,
    IAuthorizationService authorizationService,
    IHttpContextAccessor  httpContextAccessor) : IRequestHandler<DeleteBasketItemCommand>
{
    public async Task Handle(DeleteBasketItemCommand request, CancellationToken cancellationToken)
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
        
        await basketItemRepository.DeleteBasketItem(request.BasketItemId, cancellationToken);
    }
}