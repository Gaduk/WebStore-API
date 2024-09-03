namespace Application.Dto;

public record UserDto(
    string? Login,
    string? FirstName,
    string? LastName,
    string? PhoneNumber,
    string? Email,
    bool IsAdmin = false
    );