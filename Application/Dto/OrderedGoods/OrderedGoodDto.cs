namespace Application.Dto.OrderedGoods;

public class OrderedGoodDto
{
    public int OrderId { get; init; }
    public int GoodId { get; init; }
    public int Amount { get; init; }
    public string Name { get; init; }
    public float Price { get; init; }

    public OrderedGoodDto() { }

    public OrderedGoodDto(int orderId, int goodId, int amount, string name, float price)
    {
        OrderId = orderId;
        GoodId = goodId;
        Amount = amount;
        Name = name;
        Price = price;
    }
}