using Application.Features.Order.Queries.GetOrder;
using Application.Features.OrderedGood.Queries.GetAllOrderedGoods;
using Application.Features.OrderedGood.Queries.GetOrderedGoods;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers;

[ApiController]
public class OrderedGoodController(IMediator mediator, IAuthorizationService authorizationService) : ControllerBase
{
    [Authorize(Roles = "admin")]
    [HttpGet("/orderedGoods")]
    public async Task<IActionResult> GetAllOrderedGoodDtos()
    {
        var orderGoods = await mediator.Send(new GetAllOrderedGoodsQuery());
        return Ok(orderGoods);
    }
    
    [HttpGet("/orderedGoods/{orderId:int}")]
    public async Task<IActionResult> GetOrderedGoodDtos(int orderId)
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
        
        var orderGoods = await mediator.Send(new GetOrderedGoodsQuery(orderId));
        return Ok(orderGoods);
    }
}