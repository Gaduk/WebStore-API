using Application.Dto.Order;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Order.Queries.GetUserOrders;

public class GetUserOrdersQueryHandler
{
    public class GetAllOrdersQueryHandler(IOrderRepository orderRepository) : 
        IRequestHandler<GetUserOrdersQuery, List<OrderDto>>
    {
        public async Task<List<OrderDto>> Handle(GetUserOrdersQuery request, CancellationToken cancellationToken)
        {
            return await orderRepository.GetUserOrders(request.Login);
        }
    }
}