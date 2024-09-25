using MediatR;

namespace Application.Features.OrderedGood.Queries.GetOrderedGoods;

public record GetOrderedGoodsQuery(int OrderId) : IRequest<List<Domain.Entities.OrderedGood>>;