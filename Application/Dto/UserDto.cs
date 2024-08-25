namespace Application.Dto;

public record UserDto(
    string Login,
    string FirstName,
    string LastName,
    string PhoneNumber,
    bool IsAdmin = false
    );