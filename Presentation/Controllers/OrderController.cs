using Application.Features.Order.Commands.CreateOrder;
using Application.Features.Order.Commands.UpdateOrder;
using Application.Features.Order.Queries.GetOrder;
using Application.Features.Order.Queries.GetOrders;
using Application.Features.User.Queries.CheckAccessToResource;
using Application.Features.User.Queries.GetUser;
using Domain.Dto.Order;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers;

[ApiController]
public class OrderController(
    ILogger<OrderController> logger,
    IMediator mediator) : ControllerBase
{
    [Authorize(Roles = "admin")]
    [HttpPatch("/orders/{orderId:int}")]
    public async Task<IActionResult> UpdateOrderStatus(int orderId, bool isDone, CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP PATCH /orders/{orderId}", orderId);
        
        var order = await mediator.Send(new GetOrderQuery(orderId), cancellationToken);
        if (order == null)
        {
            logger.LogWarning("NotFound. Order {orderId} is not found", orderId);
            return NotFound("Order is not found");
        }
        await mediator.Send(new UpdateOrderCommand(order, isDone), cancellationToken);

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
        
        var authorizationResult = await mediator.Send(
            new CheckAccessToResourceQuery(User, order.UserName, "HaveAccess"), cancellationToken);
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
        
        var authorizationResult = await mediator.Send(
            new CheckAccessToResourceQuery(User, login, "HaveAccess"), cancellationToken);
        if (!authorizationResult.Succeeded)
        {
            logger.LogWarning("Forbidden. No access");
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        if (login != null)
        {
            var user = await mediator.Send(new GetUserQuery(login), cancellationToken);
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
    public async Task<IActionResult> CreateOrder(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP POST /orders");
        
        var authorizationResult = await mediator.Send(
            new CheckAccessToResourceQuery(User, command.UserName, "HaveAccess"), cancellationToken);
        if (!authorizationResult.Succeeded)
        {
            logger.LogWarning("Forbidden. No access");
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        var user = await mediator.Send(new GetUserQuery(command.UserName), cancellationToken);
        if (user == null)
        {
            logger.LogWarning("NotFound. User {login} is not found", command.UserName);
            return NotFound($"User {command.UserName} is not found");
        }
        
        var orderId = await mediator.Send(command, cancellationToken);
        
        logger.LogInformation("Order {orderId} is created", orderId);
        
        return CreatedAtAction(
            nameof(GetOrder), 
            new { orderId },
            new OrderDto(orderId, command.UserName,false)
        );
    }
}