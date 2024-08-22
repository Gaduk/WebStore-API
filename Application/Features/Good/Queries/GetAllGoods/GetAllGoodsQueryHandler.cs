using Application.Interfaces;
using MediatR;

namespace Application.Features.Good.Queries.GetAllGoods;

public class GetAllGoodsQueryHandler(IGoodRepository goodRepository) : 
    IRequestHandler<GetAllGoodsQuery, List<Domain.Entities.Good>>
{
    public async Task<List<Domain.Entities.Good>> Handle(GetAllGoodsQuery request, CancellationToken cancellationToken)
    {
        return await goodRepository.GetAllGoods();
    }
}