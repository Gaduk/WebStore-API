using MediatR;

namespace Application.Features.Good.Queries.GetGoods;

public record GetGoodsQuery(int? MinPrice, int? MaxPrice) : IRequest<List<Domain.Entities.Good>>;