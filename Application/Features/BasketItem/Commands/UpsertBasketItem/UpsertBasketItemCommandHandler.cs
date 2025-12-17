using Application.Exceptions;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Application.Features.BasketItem.Commands.UpsertBasketItem;

public class UpsertBasketItemCommandHandler(
    IBasketItemRepository basketItemRepository,  
    IUserRepository       userRepository,
    IAuthorizationService authorizationService,
    IHttpContextAccessor  httpContextAccessor) : IRequestHandler<UpsertBasketItemCommand>
{
    public async Task Handle(UpsertBasketItemCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUser(request.UserName, includeOrders: false, cancellationToken: cancellationToken);
        if (user == null)
        {
            throw new NotFoundException($"User {request.UserName} is not found");
        }
        
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
        
        var basketItem = new Domain.Entities.BasketItem
        {
            UserName = request.UserName,
            GoodId   = request.GoodId,
            Amount   = request.Amount
        };
        
        await basketItemRepository.UpsertBasketItem(basketItem, cancellationToken);
    }
}