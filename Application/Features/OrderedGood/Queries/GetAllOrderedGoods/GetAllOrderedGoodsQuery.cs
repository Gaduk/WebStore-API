using Application.Dto;
using Application.Dto.OrderedGoods;
using MediatR;

namespace Application.Features.OrderedGood.Queries.GetAllOrderedGoods;

public record GetAllOrderedGoodsQuery : IRequest<List<OrderedGoodDto>>;