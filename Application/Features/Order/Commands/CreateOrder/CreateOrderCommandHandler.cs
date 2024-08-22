using Application.Interfaces;
using MediatR;

namespace Application.Features.Order.Commands.CreateOrder;

public class CreateOrderCommandHandler(IOrderRepository orderRepository) : IRequestHandler<CreateOrderCommand>
{
    public async Task Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        await orderRepository.CreateOrder(request.Login, request.OrderedGoods);
    }
}