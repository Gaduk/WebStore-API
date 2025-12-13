using Application.Features.Good.Commands.CreateGood;
using Application.Features.Good.Commands.DeleteGood;
using Application.Features.Good.Commands.UpdateGood;
using Application.Features.Good.Queries.GetGood;
using Application.Features.Good.Queries.GetGoods;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
public class GoodController(ILogger<GoodController> logger, IMediator mediator) : ControllerBase
{
    [HttpGet("/goods")]
    public async Task<IActionResult> GetAllGoods(int? minPrice, int? maxPrice, CancellationToken cancellationToken)
    {
        var goods = await mediator.Send(new GetGoodsQuery(minPrice, maxPrice), cancellationToken);
        return Ok(goods);
    }
    
    [HttpGet("/goods/{goodId:int}")]
    public async Task<IActionResult> GetGood(int goodId, CancellationToken cancellationToken)
    {
        var good = await mediator.Send(new GetGoodQuery(goodId), cancellationToken);
        return Ok(good);
    }
    
    [Authorize(Roles = "admin")]
    [HttpPost("/goods")]
    public async Task<IActionResult> CreateGood([FromForm] CreateGoodCommand command, CancellationToken cancellationToken)
    {
        var goodId = await mediator.Send(command, cancellationToken);
        
        logger.LogInformation("Good {goodId} is created", goodId);
        return CreatedAtAction(nameof(GetGood), new { goodId }, null);
    }
    
    [Authorize(Roles = "admin")]
    [HttpPut("/goods")]
    public async Task<IActionResult> UpdateGood([FromForm] UpdateGoodCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        
        logger.LogInformation("Good {goodId} is updated", command.Id);
        return Ok($"Good {command.Id} is updated");
    }
    
    [Authorize(Roles = "admin")]
    [HttpDelete("/goods/{goodId:int}")]
    public async Task<IActionResult> DeleteGood(int goodId, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteGoodCommand(goodId), cancellationToken);
        
        logger.LogInformation("Good {goodId} is deleted", goodId);
        return Ok($"Good {goodId} is deleted");
    }
}