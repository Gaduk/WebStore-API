namespace Domain.Dto.Order;

public record OrderDto(
    int Id,
    string UserName,
    bool IsDone)
{
    public OrderDto() : this(default, default, default) { }
}