using Application.Dto.Order;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Order.Queries.GetAllOrders;

public class GetAllOrdersQueryHandler(IOrderRepository orderRepository) : 
    IRequestHandler<GetAllOrdersQuery, List<OrderDto>>
{
    public async Task<List<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        return await orderRepository.GetAllOrders();
    }
}