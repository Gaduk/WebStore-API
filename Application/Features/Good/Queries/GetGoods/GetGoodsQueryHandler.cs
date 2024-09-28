using Domain.Repositories;
using MediatR;

namespace Application.Features.Good.Queries.GetGoods;

public class GetGoodsQueryHandler(IGoodRepository goodRepository) : 
    IRequestHandler<GetGoodsQuery, List<Domain.Entities.Good>>
{
    public async Task<List<Domain.Entities.Good>> Handle(GetGoodsQuery request, CancellationToken cancellationToken)
    {
        return await goodRepository.GetAllGoods(cancellationToken);
    }
}