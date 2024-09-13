namespace Domain.Dto.User;

public record UserDto(
    string? Login,
    string? FirstName,
    string? LastName,
    string? PhoneNumber,
    string? Email,
    bool IsAdmin = false
);