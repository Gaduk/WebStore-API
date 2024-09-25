using Domain.Repositories;
using MediatR;

namespace Application.Features.Order.Commands.CreateOrder;

public class CreateOrderCommandHandler(IOrderRepository orderRepository) : IRequestHandler<CreateOrderCommand, int>
{
    public async Task<int> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = new Domain.Entities.Order
        {
            UserName = command.UserName
        };
        foreach (var orderedGood in command.OrderedGoods)
        {
            order.OrderedGoods.Add(new Domain.Entities.OrderedGood
            {
                OrderId = order.Id,
                GoodId = orderedGood.GoodId,
                Amount = orderedGood.Amount
            });
        }
        
        var orderId = await orderRepository.CreateOrder(order, cancellationToken);
        return orderId;
    }
}