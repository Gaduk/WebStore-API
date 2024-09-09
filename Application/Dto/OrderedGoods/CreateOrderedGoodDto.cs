namespace Application.Dto.OrderedGoods;

public class CreateOrderedGoodDto
{
    public int GoodId { get; init; }
    public int Amount { get; init; }

    public CreateOrderedGoodDto() { }

    public CreateOrderedGoodDto(int goodId, int amount)
    {
        GoodId = goodId;
        Amount = amount;
    }
}
