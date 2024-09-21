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
    IMediator mediator, 
    IAuthorizationService authorizationService,
    UserManager<User> userManager) : ControllerBase
{
    [Authorize(Roles = "admin")]
    [HttpPut("/orders/{orderId:int}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int orderId, bool isDone, CancellationToken cancellationToken)
    {
        var order = await mediator.Send(new GetOrderEntityQuery(orderId), cancellationToken);
        if (order == null)
        {
            return NotFound("Заказ не найден");
        }
        order.IsDone = isDone;
        await mediator.Send(new UpdateOrderCommand(order), cancellationToken);
        return Ok("Статус заказа обновлен");
    }
    
    [HttpGet("/orders/{orderId:int}")]
    public async Task<IActionResult> GetOrder(int orderId, CancellationToken cancellationToken)
    {
        var order = await mediator.Send(new GetOrderQuery(orderId), cancellationToken);
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
    
    [HttpGet("/orders")]
    public async Task<IActionResult> GetOrders(string? login, CancellationToken cancellationToken)
    {
        var authorizationResult = await authorizationService.AuthorizeAsync(User, login, "HaveAccess");
        if (!authorizationResult.Succeeded)
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        if (login != null)
        {
            var user = await userManager.FindByNameAsync(login);
            if (user == null)
            {
                return NotFound("Пользователь не найден");
            }
        }

        var orders = await mediator.Send(new GetOrdersQuery(login), cancellationToken);
        return Ok(orders);
    }
    
    [HttpPost("/orders")]
    public async Task<IActionResult> CreateUserOrder(string login, ShortOrderedGoodDto[] orderedGoods, CancellationToken cancellationToken)
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
        var orderId = await mediator.Send(new CreateOrderCommand(user.Id), cancellationToken);
        await mediator.Send(new CreateOrderedGoodsCommand(orderId, orderedGoods), cancellationToken);
        
        return Ok("Заказ создан");
    }
}