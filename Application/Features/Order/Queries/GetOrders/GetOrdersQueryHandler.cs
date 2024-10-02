using Application.Dto.Order;
using Application.Exceptions;
using AutoMapper;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Order.Queries.GetOrders;

public class GetOrdersQueryHandler(
    IUserRepository userRepository,
    IOrderRepository orderRepository,
    IAuthorizationService authorizationService,
    IMapper mapper,
    HttpContextAccessor httpContextAccessor) : IRequestHandler<GetOrdersQuery, List<OrderDto>>
{
    public async Task<List<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
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

        List<Domain.Entities.Order> orders;
        if (request.UserName != null)
        {
            var user = await userRepository.GetUser(request.UserName, includeOrders: true, cancellationToken: cancellationToken);
            if (user == null)
            {
                throw new NotFoundException($"User {request.UserName} is not found");
            }
            orders = user.Orders.ToList();
        }
        else orders = await orderRepository.GetOrders(cancellationToken);
        
        return mapper.Map<List<OrderDto>>(orders);
    }
}
