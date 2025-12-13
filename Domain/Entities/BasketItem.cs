namespace Domain.Entities;

public class BasketItem
{
    public Guid   Id       { get; init; } = Guid.NewGuid();
    public string UserName { get; init; } = "";
    public int    GoodId   { get; init; }
    public int    Amount   { get; set;  }

    public Good?  Good     { get; init; }
}