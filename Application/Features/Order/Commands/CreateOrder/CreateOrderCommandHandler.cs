using Application.Exceptions;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Order.Commands.CreateOrder;

public class CreateOrderCommandHandler(
    IUserRepository userRepository,
    IOrderRepository orderRepository,
    IAuthorizationService authorizationService,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<CreateOrderCommand, int>
{
    public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
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

        var user = await userRepository.GetUser(request.UserName, includeOrders: true, cancellationToken: cancellationToken);
        if (user == null)
        {
            throw new NotFoundException($"User {request.UserName} is not found");
        }
        
        var order = new Domain.Entities.Order
        {
            UserName = request.UserName
        };
        foreach (var orderedGood in request.OrderedGoods)
        {
            order.OrderedGoods.Add(new Domain.Entities.OrderedGood
            {
                OrderId = order.Id,
                GoodId = orderedGood.GoodId,
                Amount = orderedGood.Amount
            });
        }
        
        var orderId = await orderRepository.CreateOrder(order, cancellationToken);
        return orderId;
    }
}