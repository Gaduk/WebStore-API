using Domain.Repositories;
using MediatR;

namespace Application.Features.Order.Queries.GetOrderEntity;

public class GetOrderEntityQueryHandler(IOrderRepository orderRepository) : IRequestHandler<GetOrderEntityQuery, Domain.Entities.Order?>
{
    public async Task<Domain.Entities.Order?> Handle(GetOrderEntityQuery request, CancellationToken cancellationToken)
    {
        return await orderRepository.GetOrderEntity(request.OrderId);
    }
}