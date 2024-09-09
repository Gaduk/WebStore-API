namespace Application.Dto.User;

public class CreateUserDto
{
    public string Login { get; init; }
    public string Password { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string PhoneNumber { get; init; }
    public string Email { get; init; }

    public CreateUserDto() { }

    public CreateUserDto(string login, string password, string firstName, string lastName, string phoneNumber, string email)
    {
        Login = login;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Email = email;
    }
}