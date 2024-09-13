namespace Domain.Dto.User;

public record CreateUserDto(
    string? Login,
    string? Password,
    string? FirstName,
    string? LastName,
    string? PhoneNumber,
    string? Email);