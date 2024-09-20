using MediatR;

namespace Application.Features.Order.Commands.CreateOrder;

public record CreateOrderCommand(string UserId) : IRequest<int>;