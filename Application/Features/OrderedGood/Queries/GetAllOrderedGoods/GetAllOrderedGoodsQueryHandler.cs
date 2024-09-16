using Domain.Dto.OrderedGoods;
using Domain.Repositories;
using MediatR;

namespace Application.Features.OrderedGood.Queries.GetAllOrderedGoods;

public class GetAllOrderedGoodsQueryHandler(IOrderedGoodRepository orderedGoodRepository) :
    IRequestHandler<GetAllOrderedGoodsQuery, List<OrderedGoodDto>>
{
    public async Task<List<OrderedGoodDto>> Handle(GetAllOrderedGoodsQuery request, CancellationToken cancellationToken)
    {
        return await orderedGoodRepository.GetAllOrderedGoods(cancellationToken);
    }
}