using MediatR;

namespace Application.Features.Good.Queries.GetGoods;

public record GetGoodsQuery : IRequest<List<Domain.Entities.Good>>;