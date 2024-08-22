using Application.Features.Order.Commands.CreateOrder;
using Application.Features.Order.Commands.UpdateOrderStatus;
using Application.Features.Order.Queries.GetAllOrders;
using Application.Features.Order.Queries.GetOrder;
using Application.Features.Order.Queries.GetUserOrders;
using Application.Features.User.Queries.GetUserByLogin;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers;

public class OrderController(IMediator mediator) : ControllerBase
{
    [HttpPost("/{login}/order")]
    public async Task<IActionResult> GetAllGoods(string login, OrderedGood[] orderedGoods)
    {
        var user = await mediator.Send(new GetUserByLoginQuery(login));
        if (user == null)
        {
            return NotFound("Пользователь не найден");
        }
        await mediator.Send(new CreateOrderCommand(login, orderedGoods));
        return Ok("Заказ создан");
    }
    
    [HttpPut("/{orderId:int}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int orderId, bool isDone)
    {
        var order = await mediator.Send(new GetOrderQuery(orderId));
        if (order == null)
        {
            return NotFound("Заказ не найден");
        }
        await mediator.Send(new UpdateOrderStatusCommand(order, isDone));
        return Ok("Статус заказа обновлен");
    }
    
    [HttpGet("/orders")]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await mediator.Send(new GetAllOrdersQuery());
        return Ok(orders);
    }
    
    [HttpGet("/{login}/orders")]
    public async Task<IActionResult> GetUserOrders(string login)
    {
        var orders = await mediator.Send(new GetUserOrdersQuery(login));
        return Ok(orders);
    }
    
    [HttpGet("/{orderId:int}")]
    public async Task<IActionResult> GetUser(int orderId)
    {
        var order = await mediator.Send(new GetOrderQuery(orderId));
        if (order == null)
        {
            return NotFound("Заказ не найден");
        }
        return Ok(order);
    }
}