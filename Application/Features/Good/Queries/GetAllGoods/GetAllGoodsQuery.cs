using MediatR;

namespace Application.Features.Good.Queries.GetAllGoods;

public record GetAllGoodsQuery : IRequest<List<Domain.Entities.Good>>;