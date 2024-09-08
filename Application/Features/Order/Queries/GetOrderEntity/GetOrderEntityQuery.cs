using MediatR;

namespace Application.Features.Order.Queries.GetOrderEntity;

public record GetOrderEntityQuery(int OrderId) : IRequest<Domain.Entities.Order?>;