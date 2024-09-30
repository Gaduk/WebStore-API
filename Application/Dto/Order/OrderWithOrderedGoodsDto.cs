using Application.Dto.OrderedGoods;

namespace Application.Dto.Order;

public record OrderWithOrderedGoodsDto(
    int Id,
    string? UserName,
    bool IsDone,
    List<OrderedGoodWithoutOrderIdDto> OrderedGoods);