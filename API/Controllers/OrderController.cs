using Application.Dto;
using Application.Dto.Order;
using Application.Dto.OrderedGoods;
using Application.Features.Order.Commands.CreateOrder;
using Application.Features.Order.Commands.UpdateOrder;
using Application.Features.Order.Queries.GetAllOrders;
using Application.Features.Order.Queries.GetOrder;
using Application.Features.Order.Queries.GetOrderEntity;
using Application.Features.Order.Queries.GetUserOrders;
using Application.Features.User.Queries.GetUser;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers;

[ApiController]
public class OrderController(
    IMediator mediator, 
    IAuthorizationService authorizationService,
    UserManager<User> userManager) : ControllerBase
{
    [HttpPost("/order/{login}")]
    public async Task<IActionResult> CreateOrder(string login, CreateOrderedGoodDto[] orderedGoods)
    {
        var authorizationResult = await authorizationService.AuthorizeAsync(User, login, "HaveAccess");
        if (!authorizationResult.Succeeded)
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        var user = await userManager.FindByNameAsync(login);
        if (user == null)
        {
            return NotFound("Пользователь не найден");
        }
        
        await mediator.Send(new CreateOrderCommand(user.Id, orderedGoods));
        
        return Ok("Заказ создан");
    }
    
    [Authorize(Roles = "admin")]
    [HttpPut("/order/{orderId:int}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int orderId, bool isDone)
    {
        var order = await mediator.Send(new GetOrderEntityQuery(orderId));
        if (order == null)
        {
            return NotFound("Заказ не найден");
        }
        order.IsDone = isDone;
        await mediator.Send(new UpdateOrderCommand(order, isDone));
        return Ok("Статус заказа обновлен");
    }
    
    [Authorize(Roles = "admin")]
    [HttpGet("/orders")]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await mediator.Send(new GetAllOrdersQuery());
        return Ok(orders);
    }
    
    [HttpGet("/orders/{login}")]
    public async Task<IActionResult> GetUserOrders(string login)
    {
        var authorizationResult = await authorizationService.AuthorizeAsync(User, login, "HaveAccess");
        if (!authorizationResult.Succeeded)
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        
        var orders = await mediator.Send(new GetUserOrdersQuery(login));
        return Ok(orders);
    }
    
    [HttpGet("order/{orderId:int}")]
    public async Task<IActionResult> GetOrder(int orderId)
    {
        var order = await mediator.Send(new GetOrderQuery(orderId));
        if (order == null)
        {
            return NotFound("Заказ не найден");
        }
        
        var authorizationResult = await authorizationService.AuthorizeAsync(User, order.UserName, "HaveAccess");
        if (!authorizationResult.Succeeded)
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        return Ok(order);
    }
}