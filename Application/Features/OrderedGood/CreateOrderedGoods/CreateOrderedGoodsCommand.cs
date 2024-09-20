using Domain.Dto.OrderedGoods;
using MediatR;

namespace Application.Features.OrderedGood.CreateOrderedGoods;

public record CreateOrderedGoodsCommand(int OrderId, ShortOrderedGoodDto[] OrderedGoods) : IRequest;