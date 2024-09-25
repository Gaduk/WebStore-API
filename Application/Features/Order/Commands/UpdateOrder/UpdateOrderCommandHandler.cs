using Domain.Repositories;
using MediatR;

namespace Application.Features.Order.Commands.UpdateOrder;

public class UpdateOrderCommandHandler(IOrderRepository orderRepository)
    : IRequestHandler<UpdateOrderCommand>
{
    public async Task Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = request.Order;
        order.IsDone = request.IsDone;
        await orderRepository.UpdateOrder(order, cancellationToken);
    }
}