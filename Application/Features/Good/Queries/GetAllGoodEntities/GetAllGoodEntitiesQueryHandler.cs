using Application.Interfaces;
using MediatR;

namespace Application.Features.Good.Queries.GetAllGoodEntities;

public class GetAllGoodEntitiesQueryHandler(IGoodRepository goodRepository) : 
    IRequestHandler<GetAllGoodEntitiesQuery, List<Domain.Entities.Good>>
{
    public async Task<List<Domain.Entities.Good>> Handle(GetAllGoodEntitiesQuery request, CancellationToken cancellationToken)
    {
        return await goodRepository.GetAllGoods();
    }
}