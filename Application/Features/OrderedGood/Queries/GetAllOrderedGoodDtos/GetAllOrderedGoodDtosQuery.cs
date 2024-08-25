using Application.Dto;
using MediatR;

namespace Application.Features.OrderedGood.Queries.GetAllOrderedGoodDtos;

public record GetAllOrderedGoodDtosQuery : IRequest<List<OrderedGoodDto>>;