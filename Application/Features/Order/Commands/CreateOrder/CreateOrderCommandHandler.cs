using Domain.Repositories;
using MediatR;

namespace Application.Features.Order.Commands.CreateOrder;

public class CreateOrderCommandHandler(IOrderRepository orderRepository) : IRequestHandler<CreateOrderCommand, int>
{
    public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        return await orderRepository.CreateOrder(request.UserId, request.OrderedGoods);
    }
}