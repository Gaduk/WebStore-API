using Domain.Repositories;
using MediatR;

namespace Application.Features.Order.Queries.GetOrders;

public class GetOrdersQueryHandler(IOrderRepository orderRepository) : 
    IRequestHandler<GetOrdersQuery, List<Domain.Entities.Order>>
{
    public async Task<List<Domain.Entities.Order>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        return await orderRepository.GetOrders(cancellationToken);
    }
}
