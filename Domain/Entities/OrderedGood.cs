namespace Domain.Entities;

public class OrderedGood
{
    public int Id { get; set; }
    //Внешний ключ
    public int OrderId { get; set; }
    public int GoodId { get; set; }
    public int Amount { get; set; } = 0;
    
    //Навигационное свойство
    public Good Good { get; set; }
}