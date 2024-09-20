using Domain.Repositories;
using MediatR;

namespace Application.Features.OrderedGood.CreateOrderedGoods;

public class CreateOrderedGoodsCommandHandler(IOrderedGoodRepository orderedGoodRepository) : IRequestHandler<CreateOrderedGoodsCommand>
{
    public async Task Handle(CreateOrderedGoodsCommand request, CancellationToken cancellationToken)
    {
        await orderedGoodRepository.CreateOrderedGoods(request.OrderId, request.OrderedGoods, cancellationToken);
    }
}