using Application.Dto.Order;
using Application.Features.Order.Commands.CreateOrder;
using Application.Features.Order.Commands.UpdateOrder;
using Application.Features.Order.Queries.GetOrder;
using Application.Features.Order.Queries.GetOrders;
using Application.Features.User.Queries.CheckAccessToResource;
using Application.Features.User.Queries.GetUser;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
public class OrderController(
    ILogger<OrderController> logger,
    IMediator mediator,
    IMapper mapper) : ControllerBase
{
    [Authorize(Roles = "user")]
    [HttpGet("/orders")]
    public async Task<IActionResult> GetOrders(string? username, CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP GET /orders requested");
        
        var authorizationResult = await mediator.Send(
            new CheckAccessToResourceQuery(User, username, "HaveAccess"), cancellationToken);
        if (!authorizationResult.Succeeded)
        {
            logger.LogWarning("Forbidden. No access");
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        List<Order> orders;
        if (username != null)
        {
            var user = await mediator.Send(new GetUserQuery(username, IncludeOrders: true), cancellationToken);
            if (user == null)
            {
                logger.LogWarning("NotFound. User {username} is not found", username);
                return NotFound($"User {username} is not found");
            }
            orders = user.Orders.ToList();
        }
        else orders = await mediator.Send(new GetOrdersQuery(), cancellationToken);
        
        var ordersDto = mapper.Map<List<OrderDto>>(orders);
        return Ok(ordersDto);
    }
    
    [Authorize(Roles = "user")]
    [HttpPost("/orders")]
    public async Task<IActionResult> CreateOrder(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP POST /orders requested");
        
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
            logger.LogWarning("NotFound. User {username} is not found", command.UserName);
            return NotFound($"User {command.UserName} is not found");
        }
        
        var orderId = await mediator.Send(command, cancellationToken);
        
        logger.LogInformation("Order {orderId} is created", orderId);
        
        return CreatedAtAction(nameof(GetOrder), new { orderId }, null);
    }
    
    [Authorize(Roles = "admin")]
    [HttpPatch("/orders/{orderId:int}")]
    public async Task<IActionResult> UpdateOrderStatus(int orderId, bool isDone, CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP PATCH /orders/{orderId} requested", orderId);
        
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
    
    [Authorize(Roles = "user")]
    [HttpGet("/orders/{orderId:int}")]
    public async Task<IActionResult> GetOrder(int orderId, CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP GET /orders/{orderId} requested", orderId);
        
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
        
        var orderDto = mapper.Map<OrderDto>(order);
        return Ok(orderDto);
    }
}