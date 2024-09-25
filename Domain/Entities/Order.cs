namespace Domain.Entities;

public class Order
{
    public int Id { get; init; }
    public string UserName { get; init; } = null!;
    public bool IsDone { get; set; }

    public ICollection<OrderedGood> OrderedGoods { get; init; } = new List<OrderedGood>();
    public User User { get; init; } = null!;
}