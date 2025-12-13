using Application.Features.BasketItem.Commands.CreateBasketItem;
using Application.Features.BasketItem.Commands.DeleteBasketItem;
using Application.Features.BasketItem.Commands.UpdateBasketItem;
using Application.Features.BasketItem.Queries.GetBasketItem;
using Application.Features.BasketItem.Queries.GetBasketItems;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
public class BasketItemController(ILogger<BasketItemController> logger, IMediator mediator) : ControllerBase
{
    [HttpGet("/basketItems")]
    public async Task<IActionResult> GetBasketItems(string userName, CancellationToken cancellationToken)
    {
        var basketItems = await mediator.Send(new GetBasketItemsQuery(userName), cancellationToken);
        return Ok(basketItems);
    }
    
    [HttpGet("/basketItems/{basketItemId:guid}")]
    public async Task<IActionResult> GetBasketItem(Guid basketItemId, CancellationToken cancellationToken)
    {
        var basketItem = await mediator.Send(new GetBasketItemQuery(basketItemId), cancellationToken);
        return Ok(basketItem);
    }
    
    [HttpPost("/basketItems")]
    public async Task<IActionResult> CreateBasketItem(CreateBasketItemCommand command, CancellationToken cancellationToken)
    {
        var basketItemId = await mediator.Send(command, cancellationToken);
        
        logger.LogInformation("Basket item {basketItemId} is created", basketItemId);
        return CreatedAtAction(nameof(GetBasketItem), new { basketItemId }, null);
    }
    
    [HttpPut("/basketItems")]
    public async Task<IActionResult> UpdateBasketItem(UpdateBasketItemCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        
        logger.LogInformation("Basket item {basketItemId} is updated", command.BasketItemId);
        return Ok($"Basket item {command.BasketItemId} is updated");
    }
    
    [HttpDelete("/basketItems/{basketItemId:guid}")]
    public async Task<IActionResult> DeleteBasketItem(Guid basketItemId, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteBasketItemCommand(basketItemId), cancellationToken);
        
        logger.LogInformation("Basket item {basketItemId} is deleted", basketItemId);
        return Ok($"Basket item {basketItemId} is deleted");
    }
}