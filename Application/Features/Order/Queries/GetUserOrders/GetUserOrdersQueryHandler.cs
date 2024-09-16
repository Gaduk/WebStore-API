using Domain.Dto.Order;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Order.Queries.GetUserOrders;

public class GetUserOrdersQueryHandler(IOrderRepository orderRepository) : 
    IRequestHandler<GetUserOrdersQuery, List<OrderDto>>
{
    public async Task<List<OrderDto>> Handle(GetUserOrdersQuery request, CancellationToken cancellationToken)
    {
        return await orderRepository.GetUserOrders(request.Login, cancellationToken);
    }
}
