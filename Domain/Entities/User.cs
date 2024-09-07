using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsAdmin { get; set; } = false;
    
    //Навигационное свойство
    public ICollection<Order> Orders { get; set; }
}