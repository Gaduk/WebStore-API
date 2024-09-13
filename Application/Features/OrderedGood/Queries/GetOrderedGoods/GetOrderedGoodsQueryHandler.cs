using Domain.Dto.OrderedGoods;
using Domain.Repositories;
using MediatR;

namespace Application.Features.OrderedGood.Queries.GetOrderedGoods;

public class GetOrderedGoodsQueryHandler(IOrderedGoodRepository orderedGoodRepository) :
    IRequestHandler<GetOrderedGoodsQuery, List<OrderedGoodDto>>
{
    public async Task<List<OrderedGoodDto>> Handle(GetOrderedGoodsQuery request, CancellationToken cancellationToken)
    {
        return await orderedGoodRepository.GetOrderedGoods(request.OrderId);
    }
}