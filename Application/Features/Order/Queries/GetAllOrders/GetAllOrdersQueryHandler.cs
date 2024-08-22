using Application.Interfaces;
using MediatR;

namespace Application.Features.Order.Queries.GetAllOrders;

public class GetAllOrdersQueryHandler(IOrderRepository orderRepository) : 
    IRequestHandler<GetAllOrdersQuery, List<Domain.Entities.Order>>
{
    public async Task<List<Domain.Entities.Order>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        return await orderRepository.GetAllOrders();
    }
}