using MediatR;

namespace Application.Features.Order.Commands.UpdateOrder;

public record UpdateOrderCommand(Domain.Entities.Order Order) : IRequest;