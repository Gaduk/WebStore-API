using Domain.Repositories;
using MediatR;

namespace Application.Features.OrderedGood.Queries.GetOrderedGoods;

public class GetOrderedGoodsQueryHandler(IOrderedGoodRepository orderedGoodRepository) :
    IRequestHandler<GetOrderedGoodsQuery, List<Domain.Entities.OrderedGood>>
{
    public async Task<List<Domain.Entities.OrderedGood>> Handle(GetOrderedGoodsQuery request, CancellationToken cancellationToken)
    {
        return await orderedGoodRepository.GetOrderedGoods(request.OrderId, cancellationToken);
    }
}