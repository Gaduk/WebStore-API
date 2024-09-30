namespace Application.Dto.Order;

public record OrderDto(
    int Id,
    string? UserName,
    bool IsDone);