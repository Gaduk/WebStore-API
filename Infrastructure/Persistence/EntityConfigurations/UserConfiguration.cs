using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations;

public class UserConfiguration: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasMany(u => u.Orders)
            .WithOne(o => o.User)
            .HasForeignKey(o => o.UserName);
        
        var hasher = new PasswordHasher<User>();
        builder.HasData(
            new User 
            {
                UserName = "admin",
                PasswordHash = hasher.HashPassword(null!, "admin"),
                
                Id = "admin",
                FirstName = "Иван",
                LastName = "Иванов",
                IsAdmin = true,
                PhoneNumber = "+71112223344",
                NormalizedUserName = "admin".ToUpper(),
                Email = "admin@mail.ru",
                NormalizedEmail = "admin@mail.ru".ToUpper(),
                EmailConfirmed = true
                
            });
    }
}