namespace Domain.Entities;

public class OrderedGood
{
    public int Id { get; init; }
    public int OrderId { get; init; }
    public int GoodId { get; init; }
    public int Amount { get; init; }

    public Good Good { get; init; } = null!;

}