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
    [HttpGet("/ordered-goods")]
    public async Task<IActionResult> GetAllOrderedGoods(CancellationToken cancellationToken)
    {
        var orderGoods = await mediator.Send(new GetAllOrderedGoodsQuery(), cancellationToken);
        return Ok(orderGoods);
    }
    
    [HttpGet("/ordered-goods/{orderId:int}")]
    public async Task<IActionResult> GetOrderedGoods(int orderId, CancellationToken cancellationToken)
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
        
        var orderGoods = await mediator.Send(new GetOrderedGoodsQuery(orderId), cancellationToken);
        return Ok(orderGoods);
    }
}