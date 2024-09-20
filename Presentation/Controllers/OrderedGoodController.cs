using Application.Features.OrderedGood.Queries.GetAllOrderedGoods;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers;

[ApiController]
public class OrderedGoodController(IMediator mediator, IAuthorizationService authorizationService) : ControllerBase
{
    [Authorize(Roles = "admin")]
    [HttpGet("/orderedGoods")]
    public async Task<IActionResult> GetAllOrderedGoods(CancellationToken cancellationToken)
    {
        var orderGoods = await mediator.Send(new GetAllOrderedGoodsQuery(), cancellationToken);
        return Ok(orderGoods);
    }
}