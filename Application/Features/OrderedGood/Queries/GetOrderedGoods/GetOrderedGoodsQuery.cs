using Application.Dto;
using Application.Dto.OrderedGoods;
using MediatR;

namespace Application.Features.OrderedGood.Queries.GetOrderedGoods;

public record GetOrderedGoodsQuery(int OrderId) : IRequest<List<OrderedGoodDto>>;