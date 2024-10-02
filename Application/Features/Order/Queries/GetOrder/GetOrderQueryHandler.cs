using Application.Dto.Order;
using Application.Exceptions;
using AutoMapper;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Order.Queries.GetOrder;

public class GetOrderQueryHandler(
    IOrderRepository orderRepository,
    IAuthorizationService authorizationService,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetOrderQuery, OrderDto?>
{
    public async Task<OrderDto?> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetOrder(request.OrderId, cancellationToken: cancellationToken);
        if (order == null)
        {
            throw new NotFoundException($"Order №{request.OrderId} is not found");
        }
        
        var context = httpContextAccessor.HttpContext;
        if (context == null)
        {
            throw new NullReferenceException("HttpContext is null");
        }
        
        var authorizationResult = await authorizationService.AuthorizeAsync(context.User, order.UserName, "HaveAccess");
        if (!authorizationResult.Succeeded)
        {
            throw new ForbiddenException();
        }
        
        return mapper.Map<OrderDto>(order);
    }
}