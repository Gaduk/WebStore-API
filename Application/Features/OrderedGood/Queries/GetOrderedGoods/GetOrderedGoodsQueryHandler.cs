using System.Security.Claims;
using Application.Dto.OrderedGoods;
using Application.Exceptions;
using AutoMapper;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Application.Features.OrderedGood.Queries.GetOrderedGoods;

public class GetOrderedGoodsQueryHandler(
    IOrderedGoodRepository orderedGoodRepository,
    IOrderRepository orderRepository,
    IAuthorizationService authorizationService,
    IMapper mapper,
    HttpContextAccessor httpContextAccessor) :
    IRequestHandler<GetOrderedGoodsQuery, List<OrderedGoodDto>>
{
    public async Task<List<OrderedGoodDto>> Handle(GetOrderedGoodsQuery request, CancellationToken cancellationToken)
    {
        var context = httpContextAccessor.HttpContext;
        if (context == null)
        {
            throw new NullReferenceException("HttpContext is null");
        }
        
        List<Domain.Entities.OrderedGood> orderedGoods;
        if (request.OrderId == null)
        {
            var hasAdminClaim = context.User.HasClaim(ClaimTypes.Role, "admin");
            if (hasAdminClaim)
            {
                orderedGoods = await orderedGoodRepository.GetOrderedGoods(cancellationToken);
            }
            else
            {
                throw new ForbiddenException();
            }
        }
        else
        {
            var order = await orderRepository.GetOrder(
                (int)request.OrderId, 
                includeOrderedGoods: true, 
                cancellationToken: cancellationToken);
            if (order == null)
            {
                throw new NotFoundException($"Order №{request.OrderId} is not found");
            }

            var authorizationResult = await authorizationService.AuthorizeAsync(context.User, order.UserName, "HaveAccess");
            if (!authorizationResult.Succeeded)
            {
                throw new ForbiddenException();
            }
            
            orderedGoods = order.OrderedGoods.ToList();
        }

        return mapper.Map<List<OrderedGoodDto>>(orderedGoods);
    }
}