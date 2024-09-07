namespace Domain.Entities;

public class Order
{
    public int Id { get; set; }
    //Внешний ключ
    public string UserName { get; set; }
    public bool IsDone { get; set; } = false;
    
    //Навигационное свойство
    public ICollection<OrderedGood> OrderedGoods { get; set; }
    public User User { get; set; }
}