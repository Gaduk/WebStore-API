using Application.Interfaces;
using MediatR;

namespace Application.Features.Order.Queries.GetUserOrders;

public class GetUserOrdersQueryHandler
{
    public class GetAllOrdersQueryHandler(IOrderRepository orderRepository) : 
        IRequestHandler<GetUserOrdersQuery, List<Domain.Entities.Order>>
    {
        public async Task<List<Domain.Entities.Order>> Handle(GetUserOrdersQuery request, CancellationToken cancellationToken)
        {
            return await orderRepository.GetUserOrders(request.Login);
        }
    }
}