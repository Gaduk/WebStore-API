using Application.Features.Good.Queries.GetGoods;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
public class GoodController(IMediator mediator) : ControllerBase
{
    [HttpGet("/goods")]
    public async Task<IActionResult> GetAllGoods(int? minPrice, int? maxPrice, CancellationToken cancellationToken)
    {
        var goods = await mediator.Send(new GetGoodsQuery(minPrice, maxPrice), cancellationToken);
        return Ok(goods);
    }
}