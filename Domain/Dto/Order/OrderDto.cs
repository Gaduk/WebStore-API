namespace Domain.Dto.Order;

public record OrderDto(
    int Id,
    string? UserName,
    bool IsDone);