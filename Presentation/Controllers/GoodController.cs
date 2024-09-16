using Application.Features.Good.Queries.GetAllGoodEntities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers;

[ApiController]
public class GoodController(IMediator mediator) : ControllerBase
{
    [Authorize(Roles = "admin")]
    [HttpGet("/goods")]
    public async Task<IActionResult> GetAllGoods(CancellationToken cancellationToken)
    {
        var goods = await mediator.Send(new GetAllGoodEntitiesQuery(), cancellationToken);
        return Ok(goods);
    }
}