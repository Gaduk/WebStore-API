namespace Domain.Entities;

public class OrderedGood
{
    public int OrderId { get; set; }
    public int GoodId { get; set; }
    public int Amount { get; set; } = 0;
}