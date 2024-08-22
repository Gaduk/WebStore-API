using MediatR;

namespace Application.Features.OrderedGood.Queries.GetOrderedGoodDtos;

public record GetOrderedGoodDtosQuery(int OrderId) : IRequest<List<OrderedGoodDto>>;