using Application.Dto;
using Application.Dto.OrderedGoods;
using Application.Interfaces;
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