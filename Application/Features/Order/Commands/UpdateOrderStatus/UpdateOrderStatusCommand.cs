using MediatR;

namespace Application.Features.Order.Commands.UpdateOrderStatus;

public record UpdateOrderStatusCommand(Domain.Entities.Order Order, bool IsDone) : IRequest;