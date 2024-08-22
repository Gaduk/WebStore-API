using Application.Interfaces;
using MediatR;

namespace Application.Features.OrderedGood.Queries.GetAllOrderedGoodDtos;

public class GetAllOrderedGoodDtosQueryHandler(IOrderedGoodRepository orderedGoodRepository) :
    IRequestHandler<GetAllOrderedGoodDtosQuery, List<OrderedGoodDto>>
{
    public async Task<List<OrderedGoodDto>> Handle(GetAllOrderedGoodDtosQuery request, CancellationToken cancellationToken)
    {
        return await orderedGoodRepository.GetAllOrderedGoodDtos();
    }
}