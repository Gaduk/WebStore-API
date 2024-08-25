using Application.Dto;
using Application.Interfaces;
using MediatR;

namespace Application.Features.OrderedGood.Queries.GetOrderedGoodDtos;

public class GetOrderedGoodDtosQueryHandler(IOrderedGoodRepository orderedGoodRepository) :
    IRequestHandler<GetOrderedGoodDtosQuery, List<OrderedGoodDto>>
{
    public async Task<List<OrderedGoodDto>> Handle(GetOrderedGoodDtosQuery request, CancellationToken cancellationToken)
    {
        return await orderedGoodRepository.GetOrderedGoodDtos(request.OrderId);
    }
}