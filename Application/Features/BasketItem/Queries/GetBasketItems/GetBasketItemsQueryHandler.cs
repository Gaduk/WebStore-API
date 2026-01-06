using Application.Dto.BasketItem;
using Application.Exceptions;
using AutoMapper;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Application.Features.BasketItem.Queries.GetBasketItems;

public class GetBasketItemsQueryHandler(
    IBasketItemRepository basketItemRepository,  
    IUserRepository       userRepository,
    IAuthorizationService authorizationService,
    IHttpContextAccessor  httpContextAccessor,
    IMapper               mapper) : IRequestHandler<GetBasketItemsQuery, List<BasketItemDto>>
{
    public async Task<List<BasketItemDto>> Handle(GetBasketItemsQuery request, CancellationToken cancellationToken)
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
        
        var basketItems = await basketItemRepository.GetBasketItems(request.UserName, cancellationToken);
        return mapper.Map<List<BasketItemDto>>(basketItems);
    }
}