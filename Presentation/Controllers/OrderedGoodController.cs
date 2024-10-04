using Application.Features.OrderedGood.Queries.GetOrderedGoods;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
public class OrderedGoodController(
    IMediator mediator) : ControllerBase
{
    [Authorize(Roles = "user")]
    [HttpGet("/orderedGoods")]
    public async Task<IActionResult> GetOrderedGoods(int? orderId, CancellationToken cancellationToken)
    {
        var orderedGoods = await mediator.Send(new GetOrderedGoodsQuery(orderId), cancellationToken);
        return Ok(orderedGoods);
    }
}