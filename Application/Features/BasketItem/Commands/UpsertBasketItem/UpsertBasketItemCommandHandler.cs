using Application.Exceptions;
using Application.Results;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Application.Features.BasketItem.Commands.UpsertBasketItem;

public class UpsertBasketItemCommandHandler(
    IBasketItemRepository basketItemRepository,  
    IUserRepository       userRepository,
    IGoodRepository       goodRepository,
    IAuthorizationService authorizationService,
    IHttpContextAccessor  httpContextAccessor) : IRequestHandler<UpsertBasketItemCommand, UpsertResult>
{
    public async Task<UpsertResult> Handle(UpsertBasketItemCommand request, CancellationToken cancellationToken)
    {   
        var user = await userRepository.GetUser(request.UserName, cancellationToken: cancellationToken);
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
        
        var good = await goodRepository.GetGood(request.GoodId, cancellationToken);
        if (good == null)
        {
            throw new NotFoundException($"Good with ID {request.GoodId} is not found");
        }
        
        var basketItem = await basketItemRepository.GetBasketItem(request.UserName, request.GoodId, cancellationToken);
        
        if (basketItem != null)
        {
            basketItem.Amount = request.Amount;
            
            await basketItemRepository.UpdateBasketItem(basketItem, cancellationToken);
            return new UpsertResult(UpsertStatus.Updated);
        }
        
        basketItem = new Domain.Entities.BasketItem
        {
            UserName = request.UserName,
            GoodId   = request.GoodId,
            Amount   = request.Amount
        };
        await basketItemRepository.CreateBasketItem(basketItem, cancellationToken);
        return new UpsertResult(UpsertStatus.Created);
    }
}