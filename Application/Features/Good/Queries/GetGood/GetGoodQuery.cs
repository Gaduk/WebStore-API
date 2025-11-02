using MediatR;

namespace Application.Features.Good.Queries.GetGood;

public record GetGoodQuery(int GoodId) : IRequest<Domain.Entities.Good?>;