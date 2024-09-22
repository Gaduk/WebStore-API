using Application.Features.Order.Commands.CreateOrder;
using Application.Features.Order.Commands.UpdateOrder;
using Application.Features.Order.Queries.GetOrder;
using Application.Features.Order.Queries.GetOrderEntity;
using Application.Features.Order.Queries.GetOrders;
using Application.Features.OrderedGood.CreateOrderedGoods;
using Domain.Dto.OrderedGoods;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers;

[ApiController]
public class OrderController(
    ILogger<OrderController> logger,
    IMediator mediator, 
    IAuthorizationService authorizationService,
    UserManager<User> userManager) : ControllerBase
{
    [Authorize(Roles = "admin")]
    [HttpPatch("/orders/{orderId:int}")]
    public async Task<IActionResult> UpdateOrderStatus(int orderId, bool isDone, CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP PATCH /orders/{orderId}", orderId);
        
        var order = await mediator.Send(new GetOrderEntityQuery(orderId), cancellationToken);
        if (order == null)
        {
            logger.LogWarning("NotFound. Order {orderId} is not found", orderId);
            return NotFound("Order is not found");
        }
        order.IsDone = isDone;
        await mediator.Send(new UpdateOrderCommand(order), cancellationToken);

        logger.LogInformation("Status of order {orderId} is updated", order.Id);
        return Ok($"Status of order {order.Id} is updated");
    }
    
    [HttpGet("/orders/{orderId:int}")]
    public async Task<IActionResult> GetOrder(int orderId, CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP GET /orders/{orderId}", orderId);
        
        var order = await mediator.Send(new GetOrderQuery(orderId), cancellationToken);
        if (order == null)
        {
            logger.LogWarning("NotFound. Order {orderId} is not found", orderId);
            return NotFound("Order is not found");
        }
        
        var authorizationResult = await authorizationService.AuthorizeAsync(User, order.UserName, "HaveAccess");
        if (!authorizationResult.Succeeded)
        {
            logger.LogWarning("Forbidden. User have no access to order {orderId}", orderId);
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        return Ok(order);
    }
    
    [HttpGet("/orders")]
    public async Task<IActionResult> GetOrders(string? login, CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP GET /orders");
        
        var authorizationResult = await authorizationService.AuthorizeAsync(User, login, "HaveAccess");
        if (!authorizationResult.Succeeded)
        {
            logger.LogWarning("Forbidden. No access");
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        if (login != null)
        {
            var user = await userManager.FindByNameAsync(login);
            if (user == null)
            {
                logger.LogWarning("NotFound. User {login} is not found", login);
                return NotFound($"User {login} is not found");
            }
        }

        var orders = await mediator.Send(new GetOrdersQuery(login), cancellationToken);
        return Ok(orders);
    }
    
    [HttpPost("/orders")]
    public async Task<IActionResult> CreateUserOrder(string login, ShortOrderedGoodDto[] orderedGoods, CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP POST /orders");
        
        var authorizationResult = await authorizationService.AuthorizeAsync(User, login, "HaveAccess");
        if (!authorizationResult.Succeeded)
        {
            logger.LogWarning("Forbidden. No access");
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        var user = await userManager.FindByNameAsync(login);
        if (user == null)
        {
            logger.LogWarning("NotFound. User {login} is not found", login);
            return NotFound($"User {login} is not found");
        }
        var orderId = await mediator.Send(new CreateOrderCommand(user.Id), cancellationToken);
        await mediator.Send(new CreateOrderedGoodsCommand(orderId, orderedGoods), cancellationToken);
        
        logger.LogInformation("Order {orderId} is created", orderId);
        return Ok($"Order {orderId} is created");
    }
}