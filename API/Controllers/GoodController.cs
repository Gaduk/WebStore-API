using Application.Features.Good.Queries.GetAllGoods;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers;

[ApiController]
public class GoodController(IMediator mediator) : ControllerBase
{
    [HttpGet("/goods")]
    public async Task<IActionResult> GetAllGoods()
    {
        var goods = await mediator.Send(new GetAllGoodsQuery());
        return Ok(goods);
    }
}