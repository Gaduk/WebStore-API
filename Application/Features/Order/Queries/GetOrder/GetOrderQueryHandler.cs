using Application.Dto.Order;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Order.Queries.GetOrder;

public class GetOrderQueryHandler(IOrderRepository orderRepository) : IRequestHandler<GetOrderQuery, OrderDto?>
{
    public async Task<OrderDto?> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        return await orderRepository.GetOrder(request.OrderId);
    }
}