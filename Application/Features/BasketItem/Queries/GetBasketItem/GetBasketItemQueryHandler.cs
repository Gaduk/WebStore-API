using Application.Dto.BasketItem;
using Application.Exceptions;
using AutoMapper;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Application.Features.BasketItem.Queries.GetBasketItem;

public class GetBasketItemQueryHandler(
    IBasketItemRepository basketItemRepository,  
    IUserRepository       userRepository,
    IAuthorizationService authorizationService,
    IHttpContextAccessor  httpContextAccessor,
    IMapper               mapper) : IRequestHandler<GetBasketItemQuery, BasketItemDto?>
{
    public async Task<BasketItemDto?> Handle(GetBasketItemQuery request, CancellationToken cancellationToken)
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
        
        var basketItem = await basketItemRepository.GetBasketItem(request.UserName, request.GoodId, cancellationToken);
        if (basketItem == null)
        {
            throw new NotFoundException($"User {request.UserName} doesn't have a good with ID {request.GoodId} in basket");
        }
        
        return mapper.Map<BasketItemDto>(basketItem);
    }
}