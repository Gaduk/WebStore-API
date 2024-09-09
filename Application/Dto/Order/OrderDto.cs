namespace Application.Dto.Order;

public class OrderDto
{
    public int Id { get; init; }
    public string UserName { get; init; }
    public bool IsDone { get; init; }

    public OrderDto() { }

    public OrderDto(int id, string userName, bool isDone)
    {
        Id = id;
        UserName = userName;
        IsDone = isDone;
    }
}