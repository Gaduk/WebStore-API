namespace Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public string UserLogin { get; set; }
    public bool IsDone { get; set; } = false;
}