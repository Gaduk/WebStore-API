using Application.Interfaces;
using MediatR;

namespace Application.Features.Order.Queries.GetOrder;

public class GetOrderQueryHandler(IOrderRepository orderRepository) : IRequestHandler<GetOrderQuery, Domain.Entities.Order?>
{
    public async Task<Domain.Entities.Order?> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        return await orderRepository.GetOrder(request.OrderId);
    }
}