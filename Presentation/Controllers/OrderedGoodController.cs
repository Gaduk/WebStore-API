using System.Security.Claims;
using Application.Features.Order.Queries.GetOrder;
using Application.Features.OrderedGood.Queries.GetAllOrderedGoods;
using Application.Features.OrderedGood.Queries.GetOrderedGoods;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers;

[ApiController]
public class OrderedGoodController(
    ILogger<OrderedGoodController> logger, 
    IMediator mediator, 
    IAuthorizationService authorizationService) : ControllerBase
{
    [HttpGet("/orderedGoods")]
    public async Task<IActionResult> GetOrderedGoods(int? orderId, CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP GET /orderedGoods");
        
        if (orderId == null)
        {
            if (User.HasClaim(ClaimTypes.Role, "admin"))
            {
                return Ok(await mediator.Send(new GetAllOrderedGoodsQuery(), cancellationToken));
            }
            
            logger.LogWarning("Forbidden. No access");
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        
        var order = await mediator.Send(new GetOrderQuery((int)orderId), cancellationToken);
        if (order == null)
        {
            logger.LogWarning("NotFound. Order {orderId} is not found", orderId);
            return NotFound($"Order {orderId} is not found");
        }

        var authorizationResult = await authorizationService.AuthorizeAsync(User, order.UserName, "HaveAccess");
        if (!authorizationResult.Succeeded)
        {
            logger.LogWarning("Forbidden. User have no access to order {orderId}", order.Id);
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        var orderedGoods = await mediator.Send(new GetOrderedGoodsQuery((int)orderId), cancellationToken);
        return Ok(orderedGoods);
    }
}