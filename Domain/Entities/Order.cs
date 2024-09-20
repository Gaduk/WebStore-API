namespace Domain.Entities;

public class Order
{
    public int Id { get; init; }
    public string UserId { get; init; } = null!;
    public bool IsDone { get; set; }
    
    //Навигационные свойства
    public ICollection<OrderedGood>? OrderedGoods { get; init; }
    public User? User { get; init; }
}