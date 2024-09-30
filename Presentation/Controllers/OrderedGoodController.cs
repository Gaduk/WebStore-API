using Application.Dto.OrderedGoods;
using Application.Features.OrderedGood.Queries.GetOrderedGoods;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
public class OrderedGoodController(
    ILogger<OrderedGoodController> logger, 
    IMediator mediator,
    IMapper mapper) : ControllerBase
{
    [Authorize(Roles = "admin")]
    [HttpGet("/orderedGoods")]
    public async Task<IActionResult> GetOrderedGoods(int? minPrice, int? maxPrice, CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP GET /orderedGoods");

        var orderedGoods = await mediator.Send(new GetOrderedGoodsQuery(minPrice, maxPrice), cancellationToken);
        
        var orderedGoodsDto = mapper.Map<List<OrderedGoodDto>>(orderedGoods);
        return Ok(orderedGoodsDto);
    }
}