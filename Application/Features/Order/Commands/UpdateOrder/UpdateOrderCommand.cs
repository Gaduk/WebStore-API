using MediatR;

namespace Application.Features.Order.Commands.UpdateOrder;

public record UpdateOrderCommand(int OrderId, bool IsDone) : IRequest;