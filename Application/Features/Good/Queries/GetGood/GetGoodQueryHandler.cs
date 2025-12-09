using Application.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Good.Queries.GetGood;

public class GetGoodQueryHandler(IGoodRepository goodRepository) : IRequestHandler<GetGoodQuery, Domain.Entities.Good?>
{
    public async Task<Domain.Entities.Good?> Handle(GetGoodQuery request, CancellationToken cancellationToken)
    {   
        var good = await goodRepository.GetGood(request.GoodId, cancellationToken: cancellationToken);
        
        if (good == null)
        {
            throw new NotFoundException($"Good ID {request.GoodId} is not found");
        }

        return good;
    }
}