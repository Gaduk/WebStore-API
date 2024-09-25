using Domain.Repositories;
using MediatR;

namespace Application.Features.OrderedGood.Queries.GetAllOrderedGoods;

public class GetAllOrderedGoodsQueryHandler(IOrderedGoodRepository orderedGoodRepository) :
    IRequestHandler<GetAllOrderedGoodsQuery, List<Domain.Entities.OrderedGood>>
{
    public async Task<List<Domain.Entities.OrderedGood>> Handle(GetAllOrderedGoodsQuery request, CancellationToken cancellationToken)
    {
        return await orderedGoodRepository.GetAllOrderedGoods(cancellationToken);
    }
}