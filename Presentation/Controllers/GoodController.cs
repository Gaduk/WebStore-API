using Application.Features.Good.Queries.GetGoods;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
public class GoodController(ILogger<GoodController> logger, IMediator mediator) : ControllerBase
{
    [HttpGet("/goods")]
    public async Task<IActionResult> GetAllGoods(CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP GET /goods");
        
        var goods = await mediator.Send(new GetGoodsQuery(), cancellationToken);
        return Ok(goods);
    }
}