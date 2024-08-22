using Application.Features.OrderedGood.Queries.GetAllOrderedGoodDtos;
using Application.Features.OrderedGood.Queries.GetOrderedGoodDtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers;

public class OrderedGoodController(IMediator mediator) : ControllerBase
{
    [HttpGet("/orderedGoods")]
    public async Task<IActionResult> GetAllOrderedGoodDtos()
    {
        var orderGoods = await mediator.Send(new GetAllOrderedGoodDtosQuery());
        return Ok(orderGoods);
    }
    
    [HttpGet("/{order}")]
    public async Task<IActionResult> GetOrderedGoodDtos(int orderId)
    {
        var orderGoods = await mediator.Send(new GetOrderedGoodDtosQuery(orderId));
        return Ok(orderGoods);
    }
}