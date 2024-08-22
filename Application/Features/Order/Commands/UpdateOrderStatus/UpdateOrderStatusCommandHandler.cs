using Application.Interfaces;
using MediatR;

namespace Application.Features.Order.Commands.UpdateOrderStatus;

public class UpdateOrderStatusCommandHandler(IOrderRepository orderRepository)
    : IRequestHandler<UpdateOrderStatusCommand>
{
    public async Task Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        await orderRepository.UpdateOrderStatus(request.Order, request.IsDone);
    }
}