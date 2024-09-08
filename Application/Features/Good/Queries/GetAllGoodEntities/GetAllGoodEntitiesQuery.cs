using MediatR;

namespace Application.Features.Good.Queries.GetAllGoodEntities;

public record GetAllGoodEntitiesQuery : IRequest<List<Domain.Entities.Good>>;