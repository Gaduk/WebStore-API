using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User : IdentityUser
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public bool IsAdmin { get; set; }


    public ICollection<Order> Orders { get; init; } = new List<Order>();
}