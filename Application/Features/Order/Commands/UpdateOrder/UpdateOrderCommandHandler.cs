using Application.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Order.Commands.UpdateOrder;

public class UpdateOrderCommandHandler(IOrderRepository orderRepository)
    : IRequestHandler<UpdateOrderCommand>
{
    public async Task Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetOrder(request.OrderId, cancellationToken: cancellationToken);
        if (order == null)
        {
            throw new NotFoundException($"Order №{request.OrderId} is not found");
        }
        
        order.IsDone = request.IsDone;
        await orderRepository.UpdateOrder(order, cancellationToken);
    }
}