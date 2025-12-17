using Application.Features.BasketItem.Commands.DeleteBasketItem;
using Application.Features.BasketItem.Commands.UpsertBasketItem;
using Application.Features.BasketItem.Queries.GetBasketItem;
using Application.Features.BasketItem.Queries.GetBasketItems;
using Application.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
public class BasketItemController(ILogger<BasketItemController> logger, IMediator mediator) : ControllerBase
{
    [HttpGet("/users/{username}/basketItems")]
    public async Task<IActionResult> GetBasketItems(string username, CancellationToken cancellationToken)
    {
        var basketItems = await mediator.Send(new GetBasketItemsQuery(username), cancellationToken);
        return Ok(basketItems);
    }
    
    [HttpGet("/users/{username}/basketItems/{goodId:int}")]
    public async Task<IActionResult> GetBasketItem(string username, int goodId, CancellationToken cancellationToken)
    {
        var basketItem = await mediator.Send(new GetBasketItemQuery(username, goodId), cancellationToken);
        return Ok(basketItem);
    }
    
    [HttpPut("/basketItems")]
    public async Task<IActionResult> UpsertBasketItem(UpsertBasketItemCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        switch (result.status)
        {
            case UpsertStatus.Created:
                logger.LogInformation("Good with ID {goodId} is added to {username}'s basket", command.GoodId, command.UserName);
                return CreatedAtAction(nameof(GetBasketItem), new { command.UserName, command.GoodId }, null);
            
            case UpsertStatus.Updated:
                logger.LogInformation("Good with ID {goodId} is updated in {username}'s basket", command.GoodId, command.UserName);
                return Ok($"Good with ID {command.GoodId} is updated in {command.UserName}'s basket");

            default:
                return StatusCode(500);
        }
    }
    
    [HttpDelete("/users/{username}/basketItems/{goodId:int}")]
    public async Task<IActionResult> DeleteBasketItem(string username, int goodId, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteBasketItemCommand(username, goodId), cancellationToken);
        
        logger.LogInformation("Good with ID {goodId} is deleted from {username}'s basket", goodId, username);
        return Ok($"Good with ID {goodId} is deleted from {username}'s basket");
    }
}