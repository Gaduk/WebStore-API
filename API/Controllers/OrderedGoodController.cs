using Application.Features.Order.Queries.GetOrder;
using Application.Features.OrderedGood.Queries.GetAllOrderedGoodDtos;
using Application.Features.OrderedGood.Queries.GetOrderedGoodDtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers;

public class OrderedGoodController(IMediator mediator, IAuthorizationService authorizationService) : ControllerBase
{
    [Authorize(Roles = "admin")]
    [HttpGet("/orderedGoods")]
    public async Task<IActionResult> GetAllOrderedGoodDtos()
    {
        var orderGoods = await mediator.Send(new GetAllOrderedGoodDtosQuery());
        return Ok(orderGoods);
    }
    
    [HttpGet("/{orderId:int}")]
    public async Task<IActionResult> GetOrderedGoodDtos(int orderId)
    {
        var order = await mediator.Send(new GetOrderQuery(orderId));
        if (order == null)
        {
            return NotFound("Заказ не найден");
        }
        
        var authorizationResult = await authorizationService.AuthorizeAsync(User, order.UserLogin, "HaveAccess");
        if (!authorizationResult.Succeeded)
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        
        var orderGoods = await mediator.Send(new GetOrderedGoodDtosQuery(orderId));
        return Ok(orderGoods);
    }
}