using MediatR;

namespace Application.Features.OrderedGood.Queries.GetOrderedGoods;

public record GetOrderedGoodsQuery(int? MinPrice, int? MaxPrice) : IRequest<List<Domain.Entities.OrderedGood>>;