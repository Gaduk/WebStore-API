namespace Domain.Dto.OrderedGoods;

public record CreateOrderedGoodDto(
    int GoodId,
    int Amount)
{
    public CreateOrderedGoodDto() : this(default, default) { }
}
