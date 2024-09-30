namespace Application.Dto.OrderedGoods;

public record OrderedGoodWithoutOrderIdDto(
    int GoodId,
    int Amount,
    string? Name,
    float Price)
{
    public OrderedGoodWithoutOrderIdDto() : this(default, default, default, default) { }
}