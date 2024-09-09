namespace Application.Dto.User;

public class UserDto
{
    public string Login { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string PhoneNumber { get; init; }
    public string Email { get; init; }
    public bool IsAdmin { get; init; } = false; 

    public UserDto() { }

    public UserDto(string login, string firstName, string lastName, string phoneNumber, string email, bool isAdmin = false)
    {
        Login = login;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Email = email;
        IsAdmin = isAdmin;
    }
}