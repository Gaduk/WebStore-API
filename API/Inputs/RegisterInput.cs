namespace Web_API.Inputs;

public record RegisterInput(
    string Login,
    string Password,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Email);