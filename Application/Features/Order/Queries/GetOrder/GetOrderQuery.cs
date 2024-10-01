using MediatR;

namespace Application.Features.Order.Queries.GetOrder;

public record GetOrderQuery(int OrderId, bool IncludeOrderedGoods = false) : IRequest<Domain.Entities.Order?>;