using Domain.Dto.Order;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Order.Queries.GetOrders;

public class GetOrdersQueryHandler(IOrderRepository orderRepository) : 
    IRequestHandler<GetOrdersQuery, List<OrderDto>>
{
    public async Task<List<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        return await orderRepository.GetOrders(request.Login, cancellationToken);
    }
}
