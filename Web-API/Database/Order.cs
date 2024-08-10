namespace Web_API;

public class Order
{
    public int ID { get; set; }
    public string UserLogin { get; set; }
    public int GoodID { get; set; }
    public int Amount { get; set; } = 0;
    public bool IsDone { get; set; } = false;
}