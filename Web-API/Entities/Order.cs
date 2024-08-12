namespace Web_API;

public class Order
{
    public int ID { get; set; }
    public string UserLogin { get; set; }
    public bool IsDone { get; set; } = false;
}