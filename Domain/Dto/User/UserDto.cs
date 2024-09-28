namespace Domain.Dto.User;

public record UserDto(
    string? UserName,
    string? FirstName,
    string? LastName,
    string? PhoneNumber,
    string? Email,
    bool IsAdmin = false
);