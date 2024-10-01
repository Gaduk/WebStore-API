using MediatR;

namespace Application.Features.OrderedGood.Queries.GetOrderedGoods;

public record GetOrderedGoodsQuery : IRequest<List<Domain.Entities.OrderedGood>>;