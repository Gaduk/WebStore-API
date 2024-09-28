using Application.Features.Good.Queries.GetAllGoodEntities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers;

[ApiController]
public class GoodController(ILogger<GoodController> logger, IMediator mediator) : ControllerBase
{
    [HttpGet("/goods")]
    public async Task<IActionResult> GetAllGoods(CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP GET /goods");
        
        var goods = await mediator.Send(new GetAllGoodEntitiesQuery(), cancellationToken);
        return Ok(goods);
    }
}