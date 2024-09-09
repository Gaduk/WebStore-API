namespace Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public bool IsDone { get; set; } = false;
    
    //Навигационные свойства
    public ICollection<OrderedGood> OrderedGoods { get; set; }
    public User User { get; set; }
}