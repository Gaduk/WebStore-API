using Application.Features.Order.Commands.CreateOrder;
using Application.Features.Order.Commands.UpdateOrder;
using Application.Features.Order.Queries.GetOrder;
using Application.Features.Order.Queries.GetOrders;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
public class OrderController(
    ILogger<OrderController> logger,
    IMediator mediator) : ControllerBase
{
    [Authorize(Roles = "user")]
    [HttpGet("/orders")]
    public async Task<IActionResult> GetOrders(string? username, CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP GET /orders requested");
        
        var orders = await mediator.Send(new GetOrdersQuery(username), cancellationToken);
        
        return Ok(orders);
    }
    
    [Authorize(Roles = "user")]
    [HttpPost("/orders")]
    public async Task<IActionResult> CreateOrder(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP POST /orders requested");
        
        var orderId = await mediator.Send(command, cancellationToken);
        
        logger.LogInformation("Order {orderId} is created", orderId);
        return CreatedAtAction(nameof(GetOrder), new { orderId }, null);
    }
    
    [Authorize(Roles = "admin")]
    [HttpPatch("/orders/{orderId:int}")]
    public async Task<IActionResult> UpdateOrderStatus(int orderId, bool isDone, CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP PATCH /orders/{orderId} requested", orderId);
        
        await mediator.Send(new UpdateOrderCommand(orderId, isDone), cancellationToken);

        logger.LogInformation("Status of order {orderId} is updated", orderId);
        return Ok($"Status of order {orderId} is updated");
    }
    
    [Authorize(Roles = "user")]
    [HttpGet("/orders/{orderId:int}")]
    public async Task<IActionResult> GetOrder(int orderId, CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP GET /orders/{orderId} requested", orderId);
        
        var order = await mediator.Send(new GetOrderQuery(orderId), cancellationToken);
        
        return Ok(order);
    }
}