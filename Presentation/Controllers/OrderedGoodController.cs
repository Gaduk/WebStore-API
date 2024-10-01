using System.Security.Claims;
using Application.Dto.OrderedGoods;
using Application.Features.Order.Queries.GetOrder;
using Application.Features.OrderedGood.Queries.GetOrderedGoods;
using Application.Features.User.Queries.CheckAccessToResource;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
public class OrderedGoodController(
    ILogger<OrderedGoodController> logger, 
    IMediator mediator,
    IMapper mapper) : ControllerBase
{
    [HttpGet("/orderedGoods")]
    public async Task<IActionResult> GetOrderedGoods(int? orderId, CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP GET /orderedGoods");

        List<OrderedGood> orderedGoods;
        if (orderId == null)
        {
            if (User.HasClaim(ClaimTypes.Role, "admin"))
            {
                orderedGoods = await mediator.Send(new GetOrderedGoodsQuery(), cancellationToken);
            }
            else
            {
                logger.LogWarning("Forbidden. No access");
                return StatusCode(StatusCodes.Status403Forbidden);
            }
        }
        else
        {
            var order = await mediator.Send(new GetOrderQuery((int)orderId, IncludeOrderedGoods: true), cancellationToken);
            if (order == null)
            {
                logger.LogWarning("NotFound. Order {orderId} is not found", orderId);
                return NotFound($"Order {orderId} is not found");
            }

            var authorizationResult = await mediator.Send(
                new CheckAccessToResourceQuery(User, order.UserName, "HaveAccess"), cancellationToken);
            if (!authorizationResult.Succeeded)
            {
                logger.LogWarning("Forbidden. User have no access to order {orderId}", order.Id);
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            orderedGoods = order.OrderedGoods.ToList();
        }

        var orderedGoodsDto = mapper.Map<List<OrderedGoodDto>>(orderedGoods);
        return Ok(orderedGoodsDto);
    }
}