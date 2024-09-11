namespace Application.Dto.User;

public record UserDto(
    string Login,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Email,
    bool IsAdmin = false
)
{
    public UserDto() : this(default, default, default, default, default) { }
}