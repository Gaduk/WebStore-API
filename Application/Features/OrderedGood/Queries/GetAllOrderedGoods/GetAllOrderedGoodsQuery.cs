using MediatR;

namespace Application.Features.OrderedGood.Queries.GetAllOrderedGoods;

public record GetAllOrderedGoodsQuery : IRequest<List<Domain.Entities.OrderedGood>>;