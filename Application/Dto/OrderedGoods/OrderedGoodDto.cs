namespace Application.Dto.OrderedGoods;

public record OrderedGoodDto(
    int OrderId,
    int GoodId,
    int Amount,
    string? Name,
    float Price)
{
    public OrderedGoodDto() : this(default, default, default, default, default) { }
}