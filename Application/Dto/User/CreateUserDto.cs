namespace Application.Dto.User;

public record CreateUserDto(
    string Login,
    string Password,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Email)
{
    public CreateUserDto() : this(default, default, default, default, default, default) { }
}