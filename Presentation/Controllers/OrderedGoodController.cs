using System.Security.Claims;
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
    [HttpGet("/orderedGoods")]
    public async Task<IActionResult> GetOrderedGoods(int? orderId, CancellationToken cancellationToken)
    {
        if (orderId == null)
        {
            if(!User.HasClaim(ClaimTypes.Role, "admin"))
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            return Ok(await mediator.Send(new GetAllOrderedGoodsQuery(), cancellationToken));
        }
        
        var order = await mediator.Send(new GetOrderQuery((int)orderId), cancellationToken);
        if (order == null)
        {
            return NotFound("Заказ не найден");
        }

        var authorizationResult = await authorizationService.AuthorizeAsync(User, order.UserName, "HaveAccess");
        if (!authorizationResult.Succeeded)
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        var orderedGoods = await mediator.Send(new GetOrderedGoodsQuery((int)orderId), cancellationToken);
        return Ok(orderedGoods);
    }
}